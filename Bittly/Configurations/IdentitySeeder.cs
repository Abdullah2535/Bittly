using Bittly.Models;
using Microsoft.AspNetCore.Identity;

namespace Bittly.Configurations
{
    public static class IdentitySeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            // 1. Resolve the managers
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>(); 

            // 2. Define the default roles
            string[] roles = { "Admin", "User"};

            foreach (var role in roles)
            {
                // If the role does not exist, create it
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new ApplicationRole(role));
                }
            }

            //  Defining the default admin user
            string adminEmail = "abdalla@bittly.com";
            string adminName = "abdalla";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminName,
                    Email = adminEmail,
                    EmailConfirmed = true
            
                };

                // Note: In production, load this password from environment variables or Key Vault
                var result = await userManager.CreateAsync(adminUser, "Hunter_25");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    Console.WriteLine(" Admin user seeded successfully!");
                }
                else
                {
                    // THIS IS THE CRITICAL FIX: Print out exactly why it failed
                    Console.WriteLine(" Failed to seed Admin user:");
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"   - {error.Description}");
                    }
                }
                }
        }
    }
}
