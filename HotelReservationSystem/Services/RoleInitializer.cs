using HotelReservationSystem.Models;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Threading.Tasks;

namespace HotelReservationSystem.Services
{
    public static class RoleInitializer
    {
        public static async Task InitializeRolesAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            var _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roleNames = { "Administrator", "HotelManager", "Customer" };
            if (!_roleManager.Roles.Any())
            {
                foreach (var roleName in roleNames)
                {
                    if (!await _roleManager.RoleExistsAsync(roleName))
                    {
                        var role = new IdentityRole { Name = roleName };
                        await _roleManager.CreateAsync(role);
                    }
                }
            }
        }

        public static async Task InitializeAdminAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            var _userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            var adminUser = await _userManager.FindByEmailAsync("adminTestEmail@gmail.com");
            if (adminUser == null)
            {
                adminUser = new User()
                {
                    Email = "adminTestEmail@gmail.com",
                    FirstName = "Eugene",
                    LastName = "Happy",
                    PhoneNumber = "380965892349",
                    UserName = "H_E",
                };
                await _userManager.CreateAsync(adminUser, "Zxcvb1!");
            }

            if (!await _userManager.IsEmailConfirmedAsync(adminUser))
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(adminUser);
                var result = await _userManager.ConfirmEmailAsync(adminUser, token);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(adminUser, "Administrator");

                }
            }
        }
    }
}
