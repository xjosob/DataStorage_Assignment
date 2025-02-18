using Data.Entities;

namespace Business.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductEntity>> GetProductsAsync();
    }
}
