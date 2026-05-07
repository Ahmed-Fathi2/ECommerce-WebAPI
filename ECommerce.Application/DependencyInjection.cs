using ECommerce.Application.Validators;
using ECommerce.Application.Mappings;
using ECommerce.Application.Services;
using ECommerce.Application.Common.Auth;
using ECommerce.Application.Common.Settings;
using ECommerce.Application.Common.Constants;
using FluentValidation;
using FluentValidation.AspNetCore;
using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using ECommerce.Application.Contracts;

namespace ECommerce.Application
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddApplicationServices(this IServiceCollection services,IConfiguration configuration)
        {
           services.AddScoped<IProductService, ProductService>();
           services.AddScoped<ICategoryService, CategoryService>();
           services.AddScoped<ICartService, CartService>();
           services.AddScoped<IOrderService, OrderService>();
           services.AddHttpClient<IPaymentService,PaymentService>();
           services.AddScoped<IPaymentService, PaymentService>();

            services
                .AddFluentValidationAutoValidation()// in fluent validation.AspNetCore
                .AddValidatorsFromAssemblyContaining<AssemblyMarker>(); // in fluent validation DI


            // only if you want to use IMapper Interface 
            services.AddMapster(); 

            // for Custom Mapping
            var config = TypeAdapterConfig.GlobalSettings; 
            config.Scan(typeof(AssemblyMarker).Assembly);

            services.Configure<KashierSetting>
                (configuration.GetSection(nameof(KashierSetting)));


            services.Configure<BlobStorageSettings>
                (configuration.GetSection(BlobStorageSettings.SectionName));

            return services;
 
        }
    }
}
