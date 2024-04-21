using GHDProductApi.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace GHDProductApi.Infrastructure.Contexts
{
    public class InMemoryContext : DbContext
    {
        public DbSet<Product> Products => Set<Product>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=:memory:");

            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => new { e.Id });
                entity.HasIndex(e => new { e.Name, e.Brand }).IsUnique();
                entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Brand).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Price).IsRequired();

            });


            modelBuilder.Entity<Product>()
                        .HasData(
                            new List<Product>
                            {
                                new() { Id = 1, Name = "P1", Brand = "B1", Price=100 },
                                new() { Id = 2, Name = "P2", Brand = "B2", Price=110 }
                            });


            base.OnModelCreating(modelBuilder);
        }
    }
}
