using ECommerce.BLL.Abstractions.Auth;
using ECommerce.BLL.Abstractions.Settings;
using ECommerce.BLL.Managers.Auth;
using ECommerce.BLL.Managers.Cart;
using ECommerce.BLL.Managers.Category;
using ECommerce.BLL.Managers.Email;
using ECommerce.BLL.Managers.FileManager;
using ECommerce.BLL.Managers.Order;
using ECommerce.BLL.Managers.Payment;
using ECommerce.BLL.Managers.Product;
using ECommerce.Common.Constants;
using FluentValidation;
using FluentValidation.AspNetCore;
using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.BLL.ServicesExtension
{
    public static class BLLServicesExtension
    {

        public static IServiceCollection AddBLLServices(this IServiceCollection services,IConfiguration configuration)
        {
           services.AddScoped<IProductManager, ProductManager>();
           services.AddScoped<ICategoryManager, CategoryManager>();
           services.AddScoped<ICartManager, CartManager>();
           services.AddScoped<IOrderManager, OrderManager>();
           services.AddScoped<IFileManager, FileManager>();
           services.AddScoped<IAuthManager, AuthManager>();
           services.AddSingleton<IJwtProvider, JwtProvider>();
           services.AddScoped<IEmailSender, EmailSender>();
           services.AddHttpClient<IPaymentService,PaymentService>();
           services.AddScoped<IPaymentService, PaymentService>();

            services
                .AddFluentValidationAutoValidation()
                .AddValidatorsFromAssemblyContaining<AssemblyMarker>();

           services.AddMapster();
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(typeof(AssemblyMarker).Assembly);

            services.Configure<KashierSetting>
                (configuration.GetSection(nameof(KashierSetting)));

            return services;
            // Add other BLL services here
        }
    }
}
