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
        public async Task<CustomerEntity> CreateCustomer(CustomerEntity customerEntity)
        {
            await _context.Customers.AddAsync(customerEntity);
            await _context.SaveChangesAsync();
            return customerEntity;
        }

        // Read
        public async Task<IEnumerable<CustomerEntity>> GetCustomersAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<CustomerEntity> GetCustomerByIdAsync(int id)
        {
            var customerEntity = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);
            return customerEntity ?? null!;
        }

        // Update
        public async Task<CustomerEntity> UpdateCustomerAsync(CustomerEntity customerEntity)
        {
            _context.Customers.Update(customerEntity);
            await _context.SaveChangesAsync();
            return customerEntity;
        }

        // Delete
        public async Task<bool> DeleteCustomerAsync(int id)
        {
            var customerEntity = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);
            if (customerEntity == null)
            {
                return false;
            }
            _context.Customers.Remove(customerEntity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
