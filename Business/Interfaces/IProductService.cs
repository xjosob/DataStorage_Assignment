using Data.Entities;

namespace Business.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductEntity>> GetProductsAsync();
        Task<ProductEntity> CreateProductAsync(ProductEntity product);
        Task<ProductEntity> UpdateProductAsync(ProductEntity product);
        Task<ProductEntity> DeleteProductAsync(int id);
    }
}
