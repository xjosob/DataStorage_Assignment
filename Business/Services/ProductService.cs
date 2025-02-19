using Business.Interfaces;
using Data.Contexts;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public class ProductService(DataContext context) : IProductService
    {
        private readonly DataContext _context = context;

        // Create
        public async Task<ProductEntity> CreateProductAsync(ProductEntity product)
        {
            try
            {
                if (
                    product == null
                    || string.IsNullOrWhiteSpace(product.ProductName)
                    || product.Price <= 0
                )
                {
                    throw new ArgumentException("Product name and price must be valid!");
                }
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return product;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating product: {ex.Message}");
                return null!;
            }
        }

        // Read
        public async Task<IEnumerable<ProductEntity>> GetProductsAsync()
        {
            try
            {
                return await _context.Products.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting products: {ex.Message}");
                return [];
            }
        }

        // Update
        public async Task<ProductEntity> UpdateProductAsync(ProductEntity product)
        {
            try
            {
                if (
                    product == null
                    || string.IsNullOrWhiteSpace(product.ProductName)
                    || product.Price <= 0
                )
                {
                    Console.WriteLine("Product name and price must be valid!");
                    return null!;
                }
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                return product;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating product: {ex.Message}");
                return null!;
            }
        }

        // Delete
        public async Task<ProductEntity> DeleteProductAsync(int productId)
        {
            try
            {
                var productEntity =
                    await _context.Products.FirstOrDefaultAsync(p => p.Id == productId)
                    ?? throw new InvalidOperationException(
                        $"Product with ID {productId} not found."
                    );
                _context.Products.Remove(productEntity);
                await _context.SaveChangesAsync();
                return productEntity;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting product: {ex.Message}");
                return null!;
            }
        }
    }
}
