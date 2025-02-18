using Data.Entities;

namespace Business.Interfaces
{
    public interface ICustomerService
    {
        Task<CustomerEntity> CreateCustomerAsync(CustomerEntity customerEntity);
        Task<IEnumerable<CustomerEntity>> GetCustomersAsync();
        Task<CustomerEntity> GetCustomerByIdAsync(int id);
        Task<CustomerEntity> UpdateCustomerAsync(CustomerEntity customerEntity);
        Task<CustomerEntity> DeleteCustomerAsync(int id);
    }
}
