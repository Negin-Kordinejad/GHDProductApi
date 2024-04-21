using GHDProductApi.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace GHDProductApi.Infrastructure.Contexts
{
    public class InMemoryContext : DbContext
    {
        public DbSet<Product> Products => Set<Product>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("InMemoryDb");

            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => new { e.Id });
                entity.Property(p => p.Name).IsRequired();
                entity.Property(e => e.Brand).IsRequired();
                entity.Property(p => p.Price).IsRequired();
                entity.HasIndex(e => new { e.Name, e.Brand}).IsUnique();
            });


            //modelBuilder.Entity<Product>()
            //            .HasData(
            //                new List<Product>
            //                {
            //                    new Product { Id = 1, Name = "P1", Brand = "B1", Price=100 },
            //                };


            base.OnModelCreating(modelBuilder);
        }
    }
}
