using Data.Contexts;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class StatusTypeRepository(DataContext context)
    {
        private readonly DataContext _context = context;

        // Create
        public async Task<StatusTypes?> AddStatusAsync(StatusTypes statusTypeEntity)
        {
            try
            {
                await _context.StatusTypes.AddAsync(statusTypeEntity);
                await _context.SaveChangesAsync();
                return statusTypeEntity;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding the status type: {ex.Message}");
                return null;
            }
        }

        // Read
        public async Task<IEnumerable<StatusTypes>> GetStatusAsync()
        {
            try
            {
                return await _context.StatusTypes.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"An error occurred while retrieving the status types: {ex.Message}"
                );
                return [];
            }
        }

        public async Task<StatusTypes?> GetStatusByIdAsync(int id)
        {
            try
            {
                return await _context.StatusTypes.FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"An error occurred while retrieving the status type: {ex.Message}"
                );
                return null;
            }
        }

        // Update
        public async Task<StatusTypes?> UpdateAsync(StatusTypes statusTypeEntity)
        {
            try
            {
                var entity = await _context.StatusTypes.FirstOrDefaultAsync(x =>
                    x.Id == statusTypeEntity.Id
                );
                if (entity != null)
                {
                    entity.StatusName = statusTypeEntity.StatusName;
                    await _context.SaveChangesAsync();
                    return entity;
                }
                Console.WriteLine("Status type not found.");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"An error occurred while updating the status type: {ex.Message}"
                );
                return null;
            }
        }

        // Delete
        public async Task<StatusTypes?> DeleteAsync(int id)
        {
            try
            {
                var entity = await _context.StatusTypes.FirstOrDefaultAsync(x => x.Id == id);
                if (entity != null)
                {
                    _context.StatusTypes.Remove(entity);
                    await _context.SaveChangesAsync();
                    return entity;
                }
                Console.WriteLine("Status type not found.");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"An error occurred while deleting the status type: {ex.Message}"
                );
                return null;
            }
        }
    }
}
