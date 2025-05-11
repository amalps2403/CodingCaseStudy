using GAC.Application.Interfaces;
using GAC.Domain.Entities;
using GAC.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GAC.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all products from the database.
        /// </summary>
        /// <returns>A list of all products.</returns>
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        /// <summary>
        /// Retrieves a product by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the product.</param>
        /// <returns>The product if found, otherwise null.</returns>
        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return await _context.Products.FindAsync(id);
        }

        /// <summary>
        /// Creates a new product in the database.
        /// </summary>
        /// <param name="product">The product to create.</param>
        /// <returns>The created product.</returns>
        public async Task<Product> CreateAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        /// <summary>
        /// Updates an existing product in the database.
        /// </summary>
        /// <param name="product">The product with updated information.</param>
        /// <returns>True if the update was successful, otherwise false.</returns>
        public async Task<bool> UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Deletes a product from the database by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the product to delete.</param>
        /// <returns>True if the deletion was successful, otherwise false.</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            _context.Products.Remove(product);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
