using ECommerce.Application.Contracts;
using ECommerce.Application.Common.Settings;
using ECommerce.API.Abstractions;
using ECommerce.Application.Common.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ECommerce.API.ServicesExtension
{
    public static class ApiServicesExtension
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            services.AddOpenApi();


            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();





            services.AddAuthentication(option =>
              {
                  // Tell the app to use JWT Bearer authentication by default
                  option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

                  // Tell the app If the user is not authenticated, challenge them using JWT Bearer authentication and rerun a 401 Unauthorized response
                  option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
              })
              .AddJwtBearer(o =>
              {
                  o.SaveToken = true;
                  o.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuerSigningKey = true, // Validate Signature using the signing key
                      ValidateIssuer = true,
                      ValidateAudience = true,
                      ValidateLifetime = true,

                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("7K+2mXqP9vRnLwA3sYdF8hJcT5bNgUeZ1oQpWkViCxMj6rHyDs4OtBIlEuAfG0n+mXqP9vRnLwB4=")),
                      ValidIssuer = "iti",
                      ValidAudience = "iti_students"
                  };

              });


            services.AddCors(options =>
              {
                  options.AddPolicy("AllowAll", policyBuilder =>
                  {
                      policyBuilder.AllowAnyOrigin()
                                   .AllowAnyMethod()
                                   .AllowAnyHeader();
                  });
              });

            services.AddAuthorization(options =>
              {
                  options.AddPolicy("RequireAdmin", policy => policy.RequireRole(DefaultRole.Admin));
                  options.AddPolicy("RequireCustomer", policy => policy.RequireRole(DefaultRole.Customer));
              });


            return services;
        }

    }
}




