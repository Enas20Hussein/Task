using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // roles
            //string[] roleNames = { "Admin", "User" };
            //foreach (var roleName in roleNames)
            //{
            //    var roleExist = await roleManager.RoleExistsAsync(roleName);
            //    if (!roleExist)
            //    {
            //        var role = new IdentityRole(roleName);
            //        await roleManager.CreateAsync(role);
            //    }
            //}

            // admin user
            //var adminUser = await userManager.FindByEmailAsync("admin@example.com");
            //if (adminUser == null)
            //{
            //    adminUser = new ApplicationUser
            //    {
            //        UserName = "admin@example.com",
            //        Email = "admin@example.com",
            //    };

            //    await userManager.CreateAsync(adminUser, "Admin123!");
            //}

            // Assign the admin role
            //if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
            //{
            //    await userManager.AddToRoleAsync(adminUser, "Admin");
            //}
        }
    }
}
