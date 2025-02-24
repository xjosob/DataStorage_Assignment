using Data.Contexts;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class ProductRepository(DataContext context)
    {
        private readonly DataContext _context = context;

        // Create
        public async Task<ProductEntity?> AddAsync(ProductEntity productEntity)
        {
            try
            {
                await _context.Products.AddAsync(productEntity);
                await _context.SaveChangesAsync();
                return productEntity;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Database update error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding a product: {ex.Message}");
            }
            return null;
        }

        // Read
        public async Task<IEnumerable<ProductEntity>> GetAllAsync()
        {
            try
            {
                return await _context.Products.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving the products: {ex.Message}");
                return [];
            }
        }

        public async Task<ProductEntity?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving the product: {ex.Message}");
                return null;
            }
        }

        // Update
        public async Task<ProductEntity?> UpdateAsync(ProductEntity productEntity)
        {
            try
            {
                var entity = await _context.Products.FindAsync(productEntity.Id);

                if (entity != null)
                {
                    _context.Entry(entity).CurrentValues.SetValues(productEntity);
                    await _context.SaveChangesAsync();
                    return entity;
                }
                Console.WriteLine($"Product with Id {productEntity.Id} not found.");
                return null!;
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"An error occurred while updating the product with id {productEntity.Id}: {ex.Message}"
                );
                return null;
            }
        }

        // Delete
        public async Task<ProductEntity?> DeleteAsync(int id)
        {
            try
            {
                var entity = await _context.Products.FindAsync(id);
                if (entity != null)
                {
                    _context.Products.Remove(entity);
                    await _context.SaveChangesAsync();
                    return entity;
                }
                Console.WriteLine($"Product with id {id} not found.");
                return null!;
            }
            catch (Exception ex)
            {
                // Log exception
                Console.WriteLine(
                    $"An error occurred while deleting the product with ID {id}: {ex.Message}"
                );
                return null;
            }
        }
    }
}
