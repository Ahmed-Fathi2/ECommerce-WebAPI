using ECommerce.API.ServicesExtension;
using ECommerce.BLL.ServicesExtension;
using ECommerce.DAL.Seeders;
using ECommerce.DAL.SevicesExtension;

namespace ECommerce.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

          builder.Services.AddDAlServices(builder.Configuration)
                          .AddBLLServices(builder.Configuration)
                          .AddApiServices();


            var app = builder.Build();

            var scope = app.Services.CreateScope();
            var seeder = scope.ServiceProvider.GetRequiredService<IDataBaseSeeder>();

            await seeder.SeedAsync();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.MapOpenApi();
            }

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

