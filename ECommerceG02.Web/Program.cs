using ECommerceG02.Abstractions.Services;
using ECommerceG02.Configuration;
using ECommerceG02.Domian.Contacts;
using ECommerceG02.Domian.Contacts.Repos;
using ECommerceG02.Domian.Contacts.UOW;
using ECommerceG02.Domian.Models.Identity;
using ECommerceG02.Presentation.Controllers;
using ECommerceG02.Presistence.Contexts;
using ECommerceG02.Presistence.Helpers;
using ECommerceG02.Presistence.Identity.Models;
using ECommerceG02.Presistence.Repos;
using ECommerceG02.Presistence.Seed;
using ECommerceG02.Presistence.UOW;
using ECommerceG02.Services;
using ECommerceG02.Services.MappingProfiles;
using ECommerceG02.Services.Services;
using ECommerceG02.Shared.ErrorModels;
using ECommerceG02.Web.CustomMiddlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;

namespace ECommerceG02.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add DbContext for Store
            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            // Add DbContext for Identity - REMOVED (will be added by AddAuthenticationServices)
            // builder.Services.AddDbContext<StoreIdentityDbContext> is now in AddAuthenticationServices

            // Add Data Seeding
            builder.Services.AddScoped<IDataSeed, DataSeed>();

            // Add Unit of Work and Services
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IManagerServices, ManagerServices>();

            // REMOVED - Identity configuration is now in AddAuthenticationServices
            // builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            //     .AddEntityFrameworkStores<StoreIdentityDbContext>();

            // ADD AUTHENTICATION SERVICES - This replaces the Identity configuration above
            builder.Services.AddAuthenticationServices(builder.Configuration);

            // Configure API Behavior Options
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .Select(e => new Shared.ErrorModels.ValidationError
                        {
                            Field = e.Key,
                            Errors = e.Value.Errors.Select(er => er.ErrorMessage)
                        });
                    var Response = new ValidationErrorToReturn()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(Response);
                };
            });

            // Add Redis for Basket
            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            builder.Services.AddSingleton<IConnectionMultiplexer>(c =>
            {
                var configuration = ConfigurationOptions.Parse(
                    builder.Configuration.GetConnectionString("RedisConnection"),
                    true);
                return ConnectionMultiplexer.Connect(configuration);
            });

            // Add AutoMapper
            builder.Services.AddAutoMapper(p => p.AddProfile(new ProjectProfile()));
            builder.Services.AddTransient<ProductResolver>();

            // Add Controllers
            builder.Services.AddControllers().AddApplicationPart(typeof(AuthController).Assembly);

            // Add CORS (if needed for frontend)
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });

                // Or use a more restrictive policy for production:
                options.AddPolicy("Production", policy =>
                {
                    policy.WithOrigins(
                        builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ??
                        new[] { "http://localhost:3000" })
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                });
            });

            var app = builder.Build();

            // Initialize Database with Identity tables and seed data
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    // Run Identity migrations and seed Identity data (DatabaseMigrationHelper)
                    await services.InitializeDatabaseAsync();

                    // Run Store data seeding (DataSeed)
                    var objectDataSeeding = services.GetRequiredService<IDataSeed>();
                    await objectDataSeeding.DataSeedingAsync();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred during database initialization");
                }
            }

            // Configure the HTTP request pipeline
            app.UseMiddleware<CustomExceptionMiddleware>();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // Add CORS
            app.UseCors(app.Environment.IsDevelopment() ? "AllowAll" : "Production");

            // IMPORTANT: Authentication must come before Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}