using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ECommerceG02.Presistence.Contexts;
using ECommerceG02.Presistence.Identity.Models;
using ECommerceG02.Domian.Models.Identity;

namespace ECommerceG02.Presistence.Helpers
{
    public static class DatabaseMigrationHelper
    {
        public static async Task InitializeDatabaseAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<StoreIdentityDbContext>();
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                await context.Database.MigrateAsync();
                await SeedRolesAsync(roleManager);
                await SeedSuperAdminUserAsync(userManager);
                await SeedDemoUsersAsync(userManager);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while initializing the database: {ex.Message}");
                throw;
            }
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Administrator", "Manager", "Customer", "Vendor", "Support" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        private static async Task SeedSuperAdminUserAsync(UserManager<ApplicationUser> userManager)
        {
            const string superAdminEmail = "superadmin@example.com";
            const string superAdminPassword = "Admin@123456";

            if (await userManager.FindByEmailAsync(superAdminEmail) == null)
            {
                var superAdminUser = new ApplicationUser
                {
                    UserName = "superadmin",
                    Email = superAdminEmail,
                    EmailConfirmed = true,

                    FirstName = "Super",
                    LastName = "Admin",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    CreatedAt = DateTime.UtcNow.AddYears(-2),
                    LastLoginAt = DateTime.UtcNow.AddDays(-1),
                    IsActive = true,

                    MobileNumber = "+201234567890",
                    AlternativeEmail = "superadmin.alt@example.com",
                    ProfilePictureUrl = "https://example.com/images/superadmin.png",

                    PreferredLanguage = "en",
                    TimeZone = "UTC",
                    TwoFactorEnabled = false,
                    FailedLoginAttempts = 0,
                    LockoutEndDate = null,


                    Bio = "I am the Super Admin of ECommerceG02 platform.",

                    Address = new Address()
                };

                var result = await userManager.CreateAsync(superAdminUser, superAdminPassword);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Error: {error.Code} - {error.Description}");
                    }
                }
            }
        }

        private static async Task SeedDemoUsersAsync(UserManager<ApplicationUser> userManager)
        {
            var demoUsers = new[]
            {
                new { Email = "customer1@example.com", Username = "customer1", Password = "Customer@123", Role = "Customer", FirstName = "John", LastName = "Doe" },
                new { Email = "customer2@example.com", Username = "customer2", Password = "Customer@123", Role = "Customer", FirstName = "Jane", LastName = "Smith" },
                new { Email = "vendor@example.com", Username = "vendor1", Password = "Vendor@123", Role = "Vendor", FirstName = "Bob", LastName = "Wilson" },
                new { Email = "support@example.com", Username = "support1", Password = "Support@123", Role = "Support", FirstName = "Alice", LastName = "Johnson" }
            };

            int index = 1;
            foreach (var userData in demoUsers)
            {
                if (await userManager.FindByEmailAsync(userData.Email) == null)
                {
                    var user = new ApplicationUser
                    {
                        UserName = userData.Username,
                        Email = userData.Email,
                        EmailConfirmed = true,
                        FirstName = userData.FirstName,
                        LastName = userData.LastName,
                        MobileNumber = $"+2010000000{index}",
                        PreferredLanguage = "en",
                        TimeZone = "UTC",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        Bio = "Demo user",
                        AlternativeEmail = $"demo{index}@example.com",
                        ProfilePictureUrl = "https://example.com/default-profile.png",

                        LockoutEndDate = null
                    };
                    var result = await userManager.CreateAsync(user, userData.Password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, userData.Role);

                        if (userData.Role == "Customer")
                        {
                            await AddDefaultAddressAsync(user);
                        }
                    }
                    index++;
                }
            }
        }


        private static Task AddDefaultAddressAsync(ApplicationUser user)
        {
            var address = new Address
            {
                UserId = user.Id,
                AddressLine1 = "123 Main St",
                AddressLine2 = "123 Main St",
                City = "Cairo",
                State = "Cairo",
                PostalCode = "12345",
                Country = "Egypt",
                PhoneNumber = user.MobileNumber,
                IsDefault = true,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
            };

            user.Address = address;

            return Task.CompletedTask;
        }
    }

    public static class DatabaseMigrationExtensions
    {
        public static async Task<IServiceProvider> InitializeDatabaseAsync(this IServiceProvider services)
        {
            await DatabaseMigrationHelper.InitializeDatabaseAsync(services);
            return services;
        }
    }
}
