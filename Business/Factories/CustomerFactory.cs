using Business.Models;
using Data.Entities;

namespace Business.Factories
{
    public class CustomerFactory
    {
        public static CustomerEntity Create(CustomerRegistrationForm form)
        {
            return new CustomerEntity
            {
                CustomerName = form.CustomerName,
                CustomerEmail = form.CustomerEmail,
                CustomerPhone = form.CustomerPhone,
            };
        }

        public static Customer Create(CustomerEntity entity)
        {
            return new Customer
            {
                Id = entity.Id,
                CustomerName = entity.CustomerName,
                CustomerEmail = entity.CustomerEmail,
                CustomerPhone = entity.CustomerPhone,
            };
        }
    }
}
