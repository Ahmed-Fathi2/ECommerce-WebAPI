using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using ECommerce.DAL.Data.Entities;

namespace ECommerce.DAL
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {

        public AppDbContext() : base()
        {

        }

        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    var connectionString = "Server=.;Database=MVCLab2;Trusted_Connection=true;TrustServerCertificate=true";

        //    optionsBuilder.UseSqlServer(connectionString);
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }



        public override int SaveChanges()
        {
            AuditLog();
            return base.SaveChanges();
        }

        private void AuditLog()
        {
            foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;

                }
            }
        }
        //public override int SaveChanges()
        //{
        //    AuditLog();
        //    return base.SaveChanges();

        //}

        //private void AuditLog()
        //{
        //    foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
        //    {
        //        if (entry.State == EntityState.Added)
        //        {
        //            entry.Entity.CreatedAt = DateTime.UtcNow;
        //        }
        //        else if (entry.State == EntityState.Modified)
        //        {
        //            entry.Entity.UpdatedAt = DateTime.UtcNow;
        //        }
        //    }
        //}



        public DbSet<Product> Products => Set<Product>();  // /public DbSet<Product> Products { get;}
        public DbSet<Category> Categories => Set<Category>();  //    public DbSet<Category> Categories { get;}


    }
}

