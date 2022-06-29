using Computer_Store.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace Computer_Store.Data
{
    public static class ContextSeed
    {
        public static async Task SeedRolesAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Moderator.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.B_User.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.A_User.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.S_User.ToString()));
        }

        public static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            var defaultUser = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@gmail.com",
                FirstName = "Lucia",
                LastName = "Frost Plume",
                Address = "73 Tự Cường, Phường 4, Tân Bình, Hồ Chí Minh",
                PhoneNumber = "0123456789",
                Phone = "0123456789",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Admin@123");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.Moderator.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.B_User.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.A_User.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.S_User.ToString());

                    var cart = new Cart
                    {
                        UserId = defaultUser.Id
                    };
                    context.Add(cart);

                    var history = new History
                    {
                        UserId = defaultUser.Id
                    };
                    context.Add(history);
                }

            }
        }
    }
}
