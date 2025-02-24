using Data.Entities;

namespace Business.Interfaces
{
    public interface IProductService
    {
        Task<ProductEntity?> CreateProductAsync(ProductEntity product);
        Task<IEnumerable<ProductEntity>> GetProductsAsync();
        Task<ProductEntity?> GetProductByIdAsync(int id);

        Task<ProductEntity?> UpdateProductAsync(ProductEntity product);
        Task<ProductEntity?> DeleteProductAsync(int id);
    }
}
