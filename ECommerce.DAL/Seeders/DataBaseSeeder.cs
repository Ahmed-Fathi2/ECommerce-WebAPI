namespace ECommerce.DAL.Seeders
{
    public class DataBaseSeeder(IRoleSeeder roleSeeder) : IDataBaseSeeder
    {
        private readonly IRoleSeeder _roleSeeder = roleSeeder;

        public async Task SeedAsync()
        {
          await _roleSeeder.SeedAsync();
        }
    }
}
