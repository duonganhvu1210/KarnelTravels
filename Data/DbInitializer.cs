using Microsoft.AspNetCore.Identity;

namespace KarnelTravels.Data
{
    public static class DbInitializer
    {
        public const string AdminRole = "Admin";
        public const string CustomerRole = "Customer";

        public static async Task InitializeAsync(
            RoleManager<IdentityRole> roleManager,
            UserManager<Models.ApplicationUser> userManager)
        {
            // Create Roles if they don't exist
            if (!await roleManager.RoleExistsAsync(AdminRole))
            {
                await roleManager.CreateAsync(new IdentityRole(AdminRole));
            }

            if (!await roleManager.RoleExistsAsync(CustomerRole))
            {
                await roleManager.CreateAsync(new IdentityRole(CustomerRole));
            }

            // Create default Admin account if it doesn't exist
            var adminEmail = "admin@karneltravels.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var defaultAdmin = new Models.ApplicationUser
                {
                    FullName = "Administrator",
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    CreatedAt = DateTime.Now
                };

                var result = await userManager.CreateAsync(defaultAdmin, "Admin@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(defaultAdmin, AdminRole);
                }
            }
        }
    }
}
