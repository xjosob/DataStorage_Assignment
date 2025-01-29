using Data.Contexts;

namespace Data.Repositories
{
    public class StatusTypeRepository(DataContext context)
    {
        private readonly DataContext _context = context;
    }
}
