using ECommerce.Application.Contracts;
using ECommerce.Application.Common.Settings;
using ECommerce.API.ServicesExtension;
using ECommerce.Application;
using ECommerce.Infrastructure.Seeders;
using ECommerce.Infrastructure;

namespace ECommerce.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

          builder.Services.AddInfrastructureServices(builder.Configuration)
                          .AddApplicationServices(builder.Configuration)
                          .AddApiServices();


            var app = builder.Build();

            var scope = app.Services.CreateScope();
            var seeder = scope.ServiceProvider.GetRequiredService<IDataBaseSeeder>();

            await seeder.SeedAsync();


            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
                app.UseSwagger();
                app.UseSwaggerUI();
                app.MapOpenApi();
            //}

            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();


            app.MapControllers();

            app.UseExceptionHandler();

            app.Run();
        }
    }
}





