using GHDProductApi.Infrastructure.Contexts;
using GHDProductApi.Infrastructure.Entities;
using GHDProductApi.Infrastructure.Intefaces;
using Microsoft.EntityFrameworkCore;

namespace GHDProductApi.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly InMemoryContext _dbContext;

        public ProductService(InMemoryContext dbContext)
        {
            _dbContext = dbContext;
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();
        }

        public async Task<Product> AddAsync(Product product, CancellationToken cancellationToken = default)
        {
            await _dbContext.Products.AddAsync(product, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return product;
        }

        public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            var count = await _dbContext.Products.CountAsync(cancellationToken);

            return count;
        }

        public async Task<Product?> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deletedProduct = await GetAsync(id, cancellationToken);
            if (deletedProduct == null) return default;

            _dbContext.Products.Remove(deletedProduct);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return deletedProduct;
        }

        public async Task<Product?> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            Product? product = await _dbContext.Products.Where(product => product.Id == id)
                                        .FirstOrDefaultAsync(cancellationToken);

            return product;
        }

        public async Task<IEnumerable<Product?>> GetPaginatedAsync(int page, int count, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Products.Skip((page - 1) * count)
                                           .Take(count)
                                           .AsNoTracking()
                                           .ToListAsync(cancellationToken);
        }

        public async Task<Product?> UpdateAsync(Product product, CancellationToken cancellationToken = default)
        {
            var updatedProduct = await GetAsync(product.Id, cancellationToken);
            if (updatedProduct == null) return default;

            updatedProduct.Name = product.Name;
            updatedProduct.Brand = product.Brand;
            updatedProduct.Price = product.Price;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return updatedProduct;


        }
    }
}
