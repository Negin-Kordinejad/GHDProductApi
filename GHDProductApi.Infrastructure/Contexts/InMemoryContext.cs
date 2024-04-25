using GHDProductApi.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Reflection.Metadata;

namespace GHDProductApi.Infrastructure.Contexts
{
    public class InMemoryContext : DbContext
    {
        public DbSet<Product> Products => Set<Product>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localDb)\\MSSQLLocalDb;initial Catalog=GHDProductManager");
            // optionsBuilder.UseSqlite("Data Source=:memory:");

            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.ApplyConfigurationsFromAssembly(typeof(InMemoryContext).Assembly);
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => new { e.Id });
                entity.HasIndex(e => new { e.Name, e.Brand }).IsUnique();
                entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Brand).IsRequired().HasMaxLength(100);
                //entity.Property(p => p.Price).IsRequired();
                entity.OwnsOne(p => p.Price, priceBuilder =>
                {
                    priceBuilder.Property(c => c.Currency).HasMaxLength(3);
                });
            });

            modelBuilder.Entity<Product>(p =>
                 {
                     p.HasData(

                         new { Id = 1, Name = "P1", Brand = "B1" },
                         new { Id = 2, Name = "P2", Brand = "B2" }
                     );
                     p.OwnsOne(p => p.Price).HasData(
                          new { ProductId = 1, Currency = "AUD", Amount = 100m },
                          new { ProductId = 2, Currency = "AUD", Amount = 110m }
                         );
                 });


            base.OnModelCreating(modelBuilder);
        }
    }
}
