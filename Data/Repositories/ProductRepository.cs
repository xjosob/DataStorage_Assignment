using Data.Contexts;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class ProductRepository(DataContext context)
    {
        private readonly DataContext _context = context;

        // Create
        public async Task<bool> AddAsync(ProductEntity productEntity)
        {
            await _context.Products.AddAsync(productEntity);
            await _context.SaveChangesAsync();
            return true;
        }

        // Read
        public async Task<List<ProductEntity>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        // Update
        public async Task<bool> UpdateAsync(ProductEntity productEntity)
        {
            var entity = await _context.Products.FirstOrDefaultAsync(x => x.Id == productEntity.Id);
            if (entity != null)
            {
                entity.Id = productEntity.Id;
                entity.ProductName = productEntity.ProductName;
                entity.Price = productEntity.Price;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        // Delete
        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (entity != null)
            {
                _context.Products.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
