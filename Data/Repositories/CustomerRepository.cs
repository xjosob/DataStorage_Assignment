using Data.Contexts;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class CustomerRepository(DataContext context)
    {
        private readonly DataContext _context = context;

        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            return await _context.Customers.AllAsync(x => x.CustomerEmail != email);
        }

        // Create
        public async Task<CustomerEntity?> AddAsync(CustomerEntity customerEntity)
        {
            try
            {
                await _context.Customers.AddAsync(customerEntity);
                await _context.SaveChangesAsync();
                return customerEntity;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding the customer: {ex.Message}");
                return null;
            }
        }

        // Read
        public async Task<List<CustomerEntity>> GetAllAsync()
        {
            try
            {
                return await _context.Customers.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"An error occurred while retrieving the customers: {ex.Message}"
                );
                return [];
            }
        }

        public async Task<CustomerEntity?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving the customer: {ex.Message}");
                return null;
            }
        }

        // Update
        public async Task<CustomerEntity?> UpdateAsync(CustomerEntity customerEntity)
        {
            try
            {
                var entity = await _context.Customers.FirstOrDefaultAsync(x =>
                    x.Id == customerEntity.Id
                );
                if (entity != null)
                {
                    _context.Entry(entity).CurrentValues.SetValues(customerEntity);
                    await _context.SaveChangesAsync();
                    return entity;
                }
                Console.WriteLine($"Customer with id {customerEntity.Id} not found.");
                return null!;
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"An error occurred while updating the customer with ID {customerEntity.Id}: {ex.Message}"
                );
                return null;
            }
        }

        // Delete
        public async Task<CustomerEntity?> DeleteAsync(int id)
        {
            try
            {
                var entity = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);
                if (entity != null)
                {
                    _context.Customers.Remove(entity);
                    await _context.SaveChangesAsync();
                    return entity;
                }
                Console.WriteLine($"Customer with ID {id} not found.");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"An error occurred while deleting the customer with ID {id}: {ex.Message}"
                );
                return null;
            }
        }
    }
}
