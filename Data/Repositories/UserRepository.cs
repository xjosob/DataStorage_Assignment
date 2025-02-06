using Data.Contexts;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class UserRepository(DataContext context)
    {
        private readonly DataContext _context = context;

        // Create
        public async Task<bool> AddUserAsync(UserEntity userEntity)
        {
            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();
            return true;
        }

        // Read
        public async Task<List<UserEntity>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        // Update
        public async Task<bool> UpdateAsync(UserEntity userEntity)
        {
            var entity = await _context.Users.FirstOrDefaultAsync(x => x.Id == userEntity.Id);
            if (entity != null)
            {
                entity.Id = userEntity.Id;
                entity.FirstName = userEntity.FirstName;
                entity.LastName = userEntity.LastName;
                entity.Email = userEntity.Email;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        // Delete
        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (entity != null)
            {
                _context.Users.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
