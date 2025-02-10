using Data.Contexts;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class CustomerRepository(DataContext context)
    {
        private readonly DataContext _context = context;

        // Create
        public async Task<bool> AddAsync(CustomerEntity customerEntity)
        {
            await _context.Customers.AddAsync(customerEntity);
            await _context.SaveChangesAsync();
            return true;
        }

        // Read
        public async Task<List<CustomerEntity>> GetAllAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        // Update
        public async Task<bool> UpdateAsync(CustomerEntity customerEntity)
        {
            var entity = await _context.Customers.FirstOrDefaultAsync(x =>
                x.Id == customerEntity.Id
            );
            if (entity != null)
            {
                entity.Id = customerEntity.Id;
                entity.CustomerName = customerEntity.CustomerName;
                entity.CustomerEmail = customerEntity.CustomerEmail;
                entity.CustomerPhone = customerEntity.CustomerPhone;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        // Delete
        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);
            if (entity != null)
            {
                _context.Customers.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
