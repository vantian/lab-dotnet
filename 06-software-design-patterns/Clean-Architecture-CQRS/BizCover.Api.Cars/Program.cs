using BizCover.Api.Cars.Middlewares;
using BizCover.Application.Abstractions;
using BizCover.Application.Features.Cars.Commands.AddCar;
using FluentValidation;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// add mediator services for CQRS pattern
builder.Services.AddMediator(options => {
    options.ServiceLifetime = ServiceLifetime.Scoped;
});

// add DI for application and infrastructure layers
BizCover.Infrastructure.Persistences.DependencyInjection.RegisterServices(builder.Services);

// register fluent validation for request validation
builder.Services.AddValidatorsFromAssembly(typeof(BizCover.Application.Global).Assembly);
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
builder.Services.AddTransient<GlobalExceptionMiddleware>();

// register serilog for logging
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.Run();
