using Entities;

public class DbSeeder
{
    private readonly AppDbContext _context;

    public DbSeeder(AppDbContext context)
    {
        _context = context;
    }

    public void Seed()
    {
        if (!_context.Roles.Any())
        {
            var adminRole = new Roles { Name = "AdminRole" };
            var userRole = new Roles { Name = "UserRole" };

            _context.Roles.AddRange(adminRole, userRole);
            _context.SaveChanges();

            var adminPermissions = new List<RolePermissionsMapping>
            {
                new RolePermissionsMapping { RoleId = userRole.Id, Permission = PermissionsConst.UserRead },
                new RolePermissionsMapping { RoleId = adminRole.Id, Permission = PermissionsConst.UserRead },
                new RolePermissionsMapping { RoleId = adminRole.Id, Permission = PermissionsConst.UserWrite },
                new RolePermissionsMapping { RoleId = adminRole.Id, Permission = PermissionsConst.UserDelete },
                new RolePermissionsMapping { RoleId = adminRole.Id, Permission = PermissionsConst.RoleRead },
                new RolePermissionsMapping { RoleId = adminRole.Id, Permission = PermissionsConst.RoleWrite },
                new RolePermissionsMapping { RoleId = adminRole.Id, Permission = PermissionsConst.RoleDelete }
            };

            var userPermissions = new List<RolePermissionsMapping>
            {
                new RolePermissionsMapping { RoleId = userRole.Id, Permission = PermissionsConst.UserRead }
            };

            _context.RolePermissionsMappings.AddRange(adminPermissions);
            _context.RolePermissionsMappings.AddRange(userPermissions);
            _context.SaveChanges();
        }

        if (!_context.Users.Any())
        {
            var adminUser = new Users { Name = "Admin", RoleId = _context.Roles.First(r => r.Name == "AdminRole").Id };
            var normalUser = new Users { Name = "User", RoleId = _context.Roles.First(r => r.Name == "UserRole").Id };

            _context.Users.AddRange(adminUser, normalUser);
            _context.SaveChanges();
        }
    }
}