using ECommerce.DAL.Entities;
using ECommerce.DAL.Repositories.CartRepository;
using ECommerce.DAL.Repositories.CategoryRepository;
using ECommerce.DAL.Repositories.OrderRepository;
using ECommerce.DAL.Repositories.ProductRepository;
using ECommerce.DAL.Repositories.UnitOfWork;
using ECommerce.DAL.Seeders;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.DAL.SevicesExtension
{
    public static class DALservicesExtension
    {
        public static IServiceCollection AddDAlServices(this IServiceCollection services,IConfiguration configuration)
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



            return services;
        }
    }
}
