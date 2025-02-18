using Business.Interfaces;
using Data.Contexts;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public class CustomerService(DataContext context) : ICustomerService
    {
        private readonly DataContext _context = context;

        // Create
        public async Task<CustomerEntity> CreateCustomerAsync(CustomerEntity customerEntity)
        {
            try
            {
                await _context.Customers.AddAsync(customerEntity);
                await _context.SaveChangesAsync();
                return customerEntity;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Database update error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while creating a customer: {ex.Message}");
            }
            return null!;
        }

        // Read
        public async Task<IEnumerable<CustomerEntity>> GetCustomersAsync()
        {
            try
            {
                return await _context.Customers.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while getting customers: {ex.Message}");
                return [];
            }
        }

        public async Task<CustomerEntity> GetCustomerByIdAsync(int id)
        {
            try
            {
                var customerEntity = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);
                return customerEntity ?? null!;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while getting a customer: {ex.Message}");
                return null!;
            }
        }

        // Update
        public async Task<CustomerEntity> UpdateCustomerAsync(CustomerEntity customerEntity)
        {
            try
            {
                _context.Customers.Update(customerEntity);
                await _context.SaveChangesAsync();
                return customerEntity;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Database update error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating a customer: {ex.Message}");
            }
            return null!;
        }

        // Delete
        public async Task<CustomerEntity> DeleteCustomerAsync(int id)
        {
            try
            {
                var customerEntity =
                    await _context.Customers.FirstOrDefaultAsync(x => x.Id == id)
                    ?? throw new InvalidOperationException("Customer not found.");
                _context.Customers.Remove(customerEntity);
                await _context.SaveChangesAsync();
                return customerEntity;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting a customer: {ex.Message}");
                return null!;
            }
        }
    }
}
