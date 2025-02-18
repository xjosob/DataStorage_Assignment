using Business.Interfaces;
using Data.Contexts;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public class StatusService(DataContext context) : IStatusService
    {
        private readonly DataContext _context = context;

        public async Task<IEnumerable<StatusTypes>> GetStatusTypesAsync()
        {
            return await _context.StatusTypes.ToListAsync();
        }
    }
}
