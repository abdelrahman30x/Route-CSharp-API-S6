using ECommerceG02.Domian.Contacts;
using ECommerceG02.Domian.Models.Identity;
using ECommerceG02.Domian.Models.Products;
using ECommerceG02.Presistence.Contexts;
using ECommerceG02.Presistence.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerceG02.Presistence.Seed
{
    public class DataSeed : IDataSeed
    {
        private readonly StoreDbContext _context;
        private readonly StoreIdentityDbContext _identityContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DataSeed(
            StoreDbContext context,
            StoreIdentityDbContext identityContext,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _identityContext = identityContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task DataSeedingAsync()
        {
            var pendingMigration = await _context.Database.GetPendingMigrationsAsync();
            if (pendingMigration.Any())
            {
                await _context.Database.MigrateAsync();
            }

            // Seed Brands
            if (!_context.ProductBrands.Any())
            {
                var productBrandData = await File.ReadAllTextAsync(@"..\Infrastructure\ECommerceG02.Presistence\Data\brands.json");
                var productBrands = JsonSerializer.Deserialize<List<ProductBrand>>(productBrandData);
                if (productBrands != null && productBrands.Any())
                    _context.ProductBrands.AddRange(productBrands);
            }

            // Seed Types
            if (!_context.ProductTypes.Any())
            {
                var productTypeData = await File.ReadAllTextAsync(@"..\Infrastructure\ECommerceG02.Presistence\Data\types.json");
                var productTypes = JsonSerializer.Deserialize<List<ProductType>>(productTypeData);
                if (productTypes != null && productTypes.Any())
                    _context.ProductTypes.AddRange(productTypes);
            }

            // Seed Products
            if (!_context.Products.Any())
            {
                var productData = await File.ReadAllTextAsync(@"..\Infrastructure\ECommerceG02.Presistence\Data\products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productData);
                if (products != null && products.Any())
                    _context.Products.AddRange(products);
            }

            await _context.SaveChangesAsync();

            // Seed Roles
            if (!_roleManager.Roles.Any())
            {
                var roles = new List<IdentityRole>
                {
                    new IdentityRole("SuperAdmin"),
                    new IdentityRole("Admin"),
                    new IdentityRole("User")
                };

                foreach (var role in roles)
                    await _roleManager.CreateAsync(role);
            }

            //// Seed Users
            //if (!_userManager.Users.Any())
            //{
            //    var superAdmin = new ApplicationUser
            //    {
            //        UserName = "superadmin",
            //        Email = "superadmin@example.com",
            //        EmailConfirmed = true,
            //        AlternativeEmail = "superadmin.alt@example.com",
            //        Bio = "I am the Super Admin of ECommerceG02 platform.",
            //        CreatedAt = DateTime.UtcNow.AddYears(-2),
            //        DateOfBirth = new DateTime(1990, 1, 1),
            //        FirstName = "Super",
            //        LastName = "Admin",
            //        IsActive = true,
            //        ReceiveNewsletter = true,
            //        ReceivePromotions = true,
            //        TwoFactorEnabled = false,
            //        DisplayName = "SuperAdmin",
            //        MobileNumber = "+201234567890",
            //        PreferredLanguage = "en",
            //        ProfilePictureUrl = "https://example.com/images/superadmin.png"
            //    };
            //    var result = await _userManager.CreateAsync(superAdmin, "Super@123");
            //    if (result.Succeeded)
            //        await _userManager.AddToRolesAsync(superAdmin, new[] { "SuperAdmin", "Admin", "User" });

            //    var admin = new ApplicationUser
            //    {
            //        UserName = "admin",
            //        Email = "admin@example.com",
            //        EmailConfirmed = true,
            //        AlternativeEmail = "admin.alt@example.com",
            //        Bio = "I am the Admin of ECommerceG02 platform.",
            //        CreatedAt = DateTime.UtcNow.AddYears(-1),
            //        DateOfBirth = new DateTime(1992, 5, 15),
            //        FirstName = "Main",
            //        LastName = "Admin",
            //        IsActive = true,
            //        ReceiveNewsletter = true,
            //        ReceivePromotions = false,
            //        TwoFactorEnabled = false,
            //        DisplayName = "AdminUser",
            //        MobileNumber = "+201112223334",
            //        PreferredLanguage = "en",
            //        ProfilePictureUrl = "https://example.com/images/admin.png"
            //    };
            //    var adminResult = await _userManager.CreateAsync(admin, "Admin@123");
            //    if (adminResult.Succeeded)
            //        await _userManager.AddToRoleAsync(admin, "Admin");
            //}

            //await _identityContext.SaveChangesAsync();
        }
    }
}
