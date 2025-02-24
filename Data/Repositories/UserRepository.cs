using Data.Contexts;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class UserRepository(DataContext context)
    {
        private readonly DataContext _context = context;

        // Create
        public async Task<UserEntity?> AddUserAsync(UserEntity userEntity)
        {
            try
            {
                await _context.Users.AddAsync(userEntity);
                await _context.SaveChangesAsync();
                return userEntity;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding the user: {ex.Message}");
                return null;
            }
        }

        // Read
        public async Task<IEnumerable<UserEntity>> GetUsersAsync()
        {
            try
            {
                return await _context.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving the users: {ex.Message}");
                return [];
            }
        }

        public async Task<UserEntity?> GetUserByIdAsync(int id)
        {
            try
            {
                return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving the user: {ex.Message}");
                return null;
            }
        }

        // Update
        public async Task<UserEntity?> UpdateAsync(UserEntity userEntity)
        {
            try
            {
                var entity = await _context.Users.FirstOrDefaultAsync(x => x.Id == userEntity.Id);
                if (entity != null)
                {
                    entity.FirstName = userEntity.FirstName;
                    entity.LastName = userEntity.LastName;
                    entity.Email = userEntity.Email;
                    await _context.SaveChangesAsync();
                    return entity;
                }
                Console.WriteLine("Error: user not found.");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating the user: {ex.Message}");
                return null;
            }
        }

        // Delete
        public async Task<UserEntity?> DeleteAsync(int id)
        {
            try
            {
                var entity = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
                if (entity != null)
                {
                    _context.Users.Remove(entity);
                    await _context.SaveChangesAsync();
                    return entity;
                }
                Console.WriteLine("Error: user not found.");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting the user: {ex.Message}");
                return null;
            }
        }
    }
}
