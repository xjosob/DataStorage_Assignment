using Data.Contexts;

namespace Data.Repositories
{
    public class ProductRepository(DataContext context)
    {
        private readonly DataContext _context = context;
    }
}
