
using ECommerce.API.Abstractions;
using ECommerce.BLL.Abstractions.Auth;
using ECommerce.BLL.Managers.Auth;
using ECommerce.BLL.Managers.Cart;
using ECommerce.BLL.Managers.Category;
using ECommerce.BLL.Managers.Email;
using ECommerce.BLL.Managers.FileManager;
using ECommerce.BLL.Managers.Order;
using ECommerce.BLL.Managers.Product;
using ECommerce.Common.Constants;
using ECommerce.DAL;
using ECommerce.DAL.Entities;
using ECommerce.DAL.Repositories.CartRepository;
using ECommerce.DAL.Repositories.CategoryRepository;
using ECommerce.DAL.Repositories.OrderRepository;
using ECommerce.DAL.Repositories.ProductRepository;
using ECommerce.DAL.Repositories.UnitOfWork;
using ECommerce.DAL.Seeders;
using FluentValidation;
using FluentValidation.AspNetCore;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ECommerce.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            #region config
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders()
                .AddSignInManager();


            builder.Services.Configure<IdentityOptions>(options =>
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


                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            });

            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ICartRepository, CartRepository>();
            builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IProductManager, ProductManager>();
            builder.Services.AddScoped<ICategoryManager, CategoryManager>();
            builder.Services.AddScoped<ICartManager, CartManager>();
            builder.Services.AddScoped<IOrderManager, OrderManager>();
            builder.Services.AddScoped<IFileManager, FileManager>();
            builder.Services.AddScoped<IAuthManager, AuthManager>();
            builder.Services.AddSingleton<IJwtProvider, JwtProvider>();
            builder.Services.AddScoped<IEmailSender, EmailSender>();
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services
                .AddFluentValidationAutoValidation()
                .AddValidatorsFromAssemblyContaining<AssemblyMarker>();

            builder.Services.AddMapster();
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(typeof(AssemblyMarker).Assembly);



            builder.Services.AddAuthentication(option =>
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


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policyBuilder =>
                {
                    policyBuilder.AllowAnyOrigin()
                                 .AllowAnyMethod()
                                 .AllowAnyHeader();
                });
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdmin", policy => policy.RequireRole(DefaultRole.Admin));
                options.AddPolicy("RequireCustomer", policy => policy.RequireAuthenticatedUser());
            });

            #endregion
            builder.Services.AddScoped<IDataBaseSeeder, DataBaseSeeder>();
            builder.Services.AddScoped<IRoleSeeder, RoleSeeder>();

            var app = builder.Build();

            var scope = app.Services.CreateScope();
            var seeder = scope.ServiceProvider.GetRequiredService<IDataBaseSeeder>();

            await seeder.SeedAsync();


            //// Configure the HTTP request pipeline.
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

