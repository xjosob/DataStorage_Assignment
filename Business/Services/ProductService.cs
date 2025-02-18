using Business.Interfaces;
using Data.Contexts;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public class ProductService(DataContext context) : IProductService
    {
        private readonly DataContext _context = context;

        public async Task<IEnumerable<ProductEntity>> GetProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }
    }
}
