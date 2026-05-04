using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Infrastructure.Seeders
{
    public interface IDataBaseSeeder
    {
        Task SeedAsync();
    }
}

