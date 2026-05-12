using ECommerce.Application.Common.Constants;
using ECommerce.Application.Common.Settings;
using ECommerce.Application.Contracts;
using ECommerce.Application.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Application
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IOrderService, OrderService>();


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

            services.AddMemoryCache();

            return services;

        }
    }
}
