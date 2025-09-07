using System.IdentityModel.Tokens.Jwt;
using Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
string SSKey = "_my_32chars_secret_key_123457890"; // make sure it's 32 chars otherwise it will throw error"

//setup dbcontext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("InMemoryDb"));

//setup authentication
builder.Services.AddAuthentication("Bearer").AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "vantian.net",
        ValidAudience = "vantian.net",
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(SSKey))
    };
});

//setup policy based authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(PermissionsConst.UserRead, policy => policy.RequireClaim("permission", PermissionsConst.UserRead));
    options.AddPolicy(PermissionsConst.UserWrite, policy => policy.RequireClaim("permission", PermissionsConst.UserWrite));
    options.AddPolicy(PermissionsConst.UserDelete, policy => policy.RequireClaim("permission", PermissionsConst.UserDelete));
    options.AddPolicy(PermissionsConst.RoleRead, policy => policy.RequireClaim("permission", PermissionsConst.RoleRead));
    options.AddPolicy(PermissionsConst.RoleWrite, policy => policy.RequireClaim("permission", PermissionsConst.RoleWrite));
    options.AddPolicy(PermissionsConst.RoleDelete, policy => policy.RequireClaim("permission", PermissionsConst.RoleDelete));
});

//register db seeder
builder.Services.AddTransient<DbSeeder>();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    //call seeder to seed initial data
    using var scope = app.Services.CreateScope();
    var seeder = scope.ServiceProvider.GetRequiredService<DbSeeder>();
    seeder.Seed();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/Login", (string username, AppDbContext _context) =>
{
    var user = _context.Users.Include(u => u.Role).FirstOrDefault(u => u.Name == username);
    if (user == null)
    {
        return Results.Unauthorized();
    }

    var permissions = _context.RolePermissionsMappings
        .Where(rp => rp.RoleId == user.RoleId)
        .Select(rp => rp.Permission)
        .ToList();

    var claims = new List<System.Security.Claims.Claim>
    {
        new System.Security.Claims.Claim("name", user.Name),
        new System.Security.Claims.Claim("role", user.Role?.Name ?? string.Empty)
    };

    //set user permissions as claims
    foreach (var permission in permissions)
    {
        claims.Add(new System.Security.Claims.Claim("permission", permission));
    }

    var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(SSKey));
    var token = new JwtSecurityToken(
        issuer: "vantian.net",
        audience: "vantian.net",
        claims: claims,
        expires: DateTime.Now.AddHours(1),
        signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
    );

    return Results.Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
}).AllowAnonymous();

app.MapGet("/Users", (AppDbContext _context) =>
{
    var users = _context.Users.ToList();
    return users;
}).RequireAuthorization(PermissionsConst.UserRead);

app.MapGet("/Users/{id}", (int id, AppDbContext _context) =>
{
    var user = _context.Users.Find(id);
    if (user == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(user);
}).RequireAuthorization(PermissionsConst.UserRead);

app.MapPost("/Users", (UserCreateDto userDto, AppDbContext _context) =>
{
    Users user = new Users { Name = userDto.Name, RoleId = userDto.RoleId };
    _context.Users.Add(user);
    _context.SaveChanges();
    return Results.Created($"/Users/{user.Id}", user);
}).RequireAuthorization(PermissionsConst.UserWrite);

app.MapPut("/Users/{id}", (int id, Users updatedUser, AppDbContext _context) =>
{
    var user = _context.Users.Find(id);
    if (user == null)
    {
        return Results.NotFound();
    }
    user.Name = updatedUser.Name;
    user.RoleId = updatedUser.RoleId;
    _context.SaveChanges();
    return Results.NoContent();
}).RequireAuthorization(PermissionsConst.UserWrite);

app.MapDelete("/Users/{id}", (int id, AppDbContext _context) =>
{
    var user = _context.Users.Find(id);
    if (user == null)
    {
        return Results.NotFound();
    }
    _context.Users.Remove(user);
    _context.SaveChanges();
    return Results.NoContent();
}).RequireAuthorization(PermissionsConst.UserDelete);


app.Run();

public record UserCreateDto(string Name, int? RoleId);