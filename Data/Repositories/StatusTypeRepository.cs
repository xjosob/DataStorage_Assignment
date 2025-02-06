using Data.Contexts;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class StatusTypeRepository(DataContext context)
    {
        private readonly DataContext _context = context;

        // Create
        public async Task<bool> AddStatusAsync(StatusTypeEntity statusTypeEntity)
        {
            await _context.StatusTypes.AddAsync(statusTypeEntity);
            await _context.SaveChangesAsync();
            return true;
        }

        // Read
        public async Task<List<StatusTypeEntity>> GetStatusAsync()
        {
            return await _context.StatusTypes.ToListAsync();
        }

        // Update
        public async Task<bool> UpdateAsync(StatusTypeEntity statusTypeEntity)
        {
            var entity = await _context.StatusTypes.FirstOrDefaultAsync(x =>
                x.Id == statusTypeEntity.Id
            );
            if (entity != null)
            {
                entity.Id = statusTypeEntity.Id;
                entity.StatusName = statusTypeEntity.StatusName;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
