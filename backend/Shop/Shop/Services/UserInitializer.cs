using Microsoft.AspNetCore.Identity;

namespace Shop.Services;

public class UserInitializer
{
    public static async Task SeedAdminUserAsync(UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        string adminUserName = "admin@example.com";
        string adminEmail = "admin@example.com";
        string adminPassword = "Admin123!";

        var user = await userManager.FindByNameAsync(adminUserName);

        if (user == null)
        {
            user = new IdentityUser()
            {
                UserName = adminUserName,
                Email = adminEmail
            };

            var result = await userManager.CreateAsync(user, adminPassword);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
}
