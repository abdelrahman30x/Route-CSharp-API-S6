using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ECommerceG02.Abstractions.Services;
using ECommerceG02.Services;
using ECommerceG02.Presistence.Contexts;
using ECommerceG02.Presistence.Identity.Models;
using ECommerceG02.Domian.Models.Identity;

namespace ECommerceG02.Configuration
{
    public static class AuthenticationServiceConfiguration
    {
        public static IServiceCollection AddAuthenticationServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<StoreIdentityDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("IdentityConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // Password settings
                options.Password.RequireDigit = configuration.GetValue<bool>("Identity:Password:RequireDigit");
                options.Password.RequiredLength = configuration.GetValue<int>("Identity:Password:RequiredLength");
                options.Password.RequireNonAlphanumeric = configuration.GetValue<bool>("Identity:Password:RequireNonAlphanumeric");
                options.Password.RequireUppercase = configuration.GetValue<bool>("Identity:Password:RequireUppercase");
                options.Password.RequireLowercase = configuration.GetValue<bool>("Identity:Password:RequireLowercase");
                options.Password.RequiredUniqueChars = configuration.GetValue<int>("Identity:Password:RequiredUniqueChars");

                // User settings
                options.User.RequireUniqueEmail = configuration.GetValue<bool>("Identity:User:RequireUniqueEmail");

                // SignIn settings
                options.SignIn.RequireConfirmedEmail = configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedEmail");
                options.SignIn.RequireConfirmedPhoneNumber = configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedPhoneNumber");
                options.SignIn.RequireConfirmedAccount = configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedAccount");

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.Parse(configuration["Identity:Lockout:DefaultLockoutTimeSpan"]);
                options.Lockout.MaxFailedAccessAttempts = configuration.GetValue<int>("Identity:Lockout:MaxFailedAccessAttempts");
                options.Lockout.AllowedForNewUsers = configuration.GetValue<bool>("Identity:Lockout:AllowedForNewUsers");
            })
            .AddEntityFrameworkStores<StoreIdentityDbContext>()
            .AddDefaultTokenProviders();

            // Configure JWT Authentication
            var jwtSecretKey = configuration["Jwt:SecretKey"];
            var jwtIssuer = configuration["Jwt:Issuer"];
            var jwtAudience = configuration["Jwt:Audience"];
            var key = Encoding.UTF8.GetBytes(jwtSecretKey);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = jwtIssuer,
                    ValidateAudience = true,
                    ValidAudience = jwtAudience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddHttpContextAccessor();

            services.AddScoped<IAuthenticationServices, AuthenticationServices>();

            return services;
        }
    }
}