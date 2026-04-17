using Microsoft.AspNetCore.Identity;

namespace ECommerce.DAL.Seeders
{
    public class RoleSeeder(AppDbContext dbContext, RoleManager<IdentityRole> roleManager) : IRoleSeeder
    {
        private readonly AppDbContext _dbContext = dbContext;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly List<string> _roles = ["Admin", "Customer" ];
        public async Task SeedAsync()
        {
            if(await _dbContext.Database.CanConnectAsync())
            {
                if(!_dbContext.Roles.Any())
                {
                    foreach (var role in _roles)
                    {
                        await _roleManager.CreateAsync(new IdentityRole (role));
                    }

                }

            }
        }
    }
}
