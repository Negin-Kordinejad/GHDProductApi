using GHDProductApi.Infrastructure.Entities;

namespace GHDProductApi.Infrastructure.Intefaces
{
    public interface IProductService
    {
        /// <summary>
        /// Gets a product from the database.
        /// </summary>
        /// <param name="id">The product id.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The product.</returns>
        Task<Product?> GetAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds products with the 
        /// </summary>
       
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The list of matching products.</returns>
        Task<IEnumerable<Product>> FindAsync(string? name, string? barand, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a page of products.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="count">The number of products per page.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The products.</returns>
        Task<IEnumerable<Product>> GetPaginatedAsync(int page, int count, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds a product to the database.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The added product.</returns>
        Task<Product> AddAsync(Product product, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing product in the database.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The updated product.</returns>
        Task<Product> UpdateAsync(Product product, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes an existing product in the database.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The deleted product.</returns>
        Task<Product?> DeleteAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lists the number of products in the database.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The number of products in the db.</returns>
        Task<int> CountAsync(CancellationToken cancellationToken = default);
    }
}
