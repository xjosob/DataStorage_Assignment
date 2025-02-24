using Business.Interfaces;
using Data.Entities;
using Data.Repositories;

namespace Business.Services
{
    public class ProductService(ProductRepository productRepository) : IProductService
    {
        private readonly ProductRepository _productRepository = productRepository;

        // Create
        public async Task<ProductEntity?> CreateProductAsync(ProductEntity product)
        {
            return await _productRepository.AddAsync(product);
        }

        // Read
        public async Task<IEnumerable<ProductEntity>> GetProductsAsync()
        {
            return await _productRepository.GetAllAsync();
        }

        public async Task<ProductEntity?> GetProductByIdAsync(int productId)
        {
            return await _productRepository.GetByIdAsync(productId);
        }

        // Update
        public async Task<ProductEntity?> UpdateProductAsync(ProductEntity product)
        {
            return await _productRepository.UpdateAsync(product);
        }

        // Delete
        public async Task<ProductEntity?> DeleteProductAsync(int productId)
        {
            return await _productRepository.DeleteAsync(productId);
        }
    }
}
