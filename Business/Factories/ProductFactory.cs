using Business.Models;
using Data.Entities;

namespace Business.Factories
{
    public class ProductFactory
    {
        public static ProductEntity CreateProduct(string productName, decimal price)
        {
            if (string.IsNullOrWhiteSpace(productName) || price <= 0)
            {
                throw new ArgumentException("Product name and price must be valid!");
            }
            return new ProductEntity { ProductName = productName, Price = price };
        }

        public static Product CreateProduct(ProductEntity entity)
        {
            return new Product
            {
                Id = entity.Id,
                ProductName = entity.ProductName,
                Price = entity.Price,
            };
        }
    }
}
