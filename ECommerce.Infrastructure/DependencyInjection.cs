using ECommerce.Infrastructure.Data;
using ECommerce.Domain.Repositories;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Seeders;
using ECommerce.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Application.Contracts;
using ECommerce.Infrastructure.Authentication;
using ECommerce.Application.Common.Auth;

namespace ECommerce.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,IConfiguration configuration)
        {
         services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
         services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders()
                .AddSignInManager();



         services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

                options.User.RequireUniqueEmail = false;


                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            });


         services.AddScoped<IProductRepository, ProductRepository>();
         services.AddScoped<ICategoryRepository, CategoryRepository>();
         services.AddScoped<ICartRepository, CartRepository>();
         services.AddScoped<ICartItemRepository, CartItemRepository>();
         services.AddScoped<IOrderRepository, OrderRepository>();
         services.AddScoped<IUnitOfWork, UnitOfWork>();
         services.AddScoped<IDataBaseSeeder, DataBaseSeeder>();
         services.AddScoped<IRoleSeeder, RoleSeeder>();



            services.AddScoped<IFileService, ECommerce.Infrastructure.ExternalServices.FileService>();
         services.AddScoped<IEmailSender, ECommerce.Infrastructure.ExternalServices.EmailSender>();
         services.AddScoped<IBlobStorageService, ECommerce.Infrastructure.ExternalServices.BlobStorageService>();

         services.AddScoped<IAuthService, AuthService>();
         services.AddSingleton<IJwtProvider, JwtProvider>();

         return services;
        }
    }
}








