using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.DAL.Seeders
{
    public interface IDataBaseSeeder
    {
        Task SeedAsync();
    }
}
