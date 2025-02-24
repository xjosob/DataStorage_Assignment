using Business.Interfaces;
using Data.Entities;
using Data.Repositories;

namespace Business.Services
{
    public class CustomerService(CustomerRepository customerRepository) : ICustomerService
    {
        private readonly CustomerRepository _customerRepository = customerRepository;

        // Create
        public async Task<CustomerEntity?> CreateCustomerAsync(CustomerEntity customerEntity)
        {
            bool isEmailUniqueAsync = await _customerRepository.IsEmailUniqueAsync(
                customerEntity.CustomerEmail
            );
            if (!isEmailUniqueAsync)
            {
                throw new InvalidOperationException("Email is already in use.");
            }
            return await _customerRepository.AddAsync(customerEntity);
        }

        // Read
        public async Task<IEnumerable<CustomerEntity>> GetCustomersAsync()
        {
            return await _customerRepository.GetAllAsync();
        }

        public async Task<CustomerEntity?> GetCustomerByIdAsync(int id)
        {
            return await _customerRepository.GetByIdAsync(id);
        }

        // Update
        public async Task<CustomerEntity?> UpdateCustomerAsync(CustomerEntity customerEntity)
        {
            return await _customerRepository.UpdateAsync(customerEntity);
        }

        // Delete
        public async Task<CustomerEntity?> DeleteCustomerAsync(int id)
        {
            return await _customerRepository.DeleteAsync(id);
        }
    }
}
