using Business.Interfaces;
using Data.Contexts;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public class UserService(DataContext context) : IUserService
    {
        private readonly DataContext _context = context;

        // Create
        public async Task<UserEntity> CreateUserAsync(UserEntity userEntity)
        {
            try
            {
                await _context.Users.AddAsync(userEntity);
                await _context.SaveChangesAsync();
                return userEntity;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Database update error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while creating a user: {ex.Message}");
            }
            return null!;
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
                Console.WriteLine($"An error occurred while getting users: {ex.Message}");
                return [];
            }
        }

        public async Task<UserEntity> GetUserByIdAsync(int id)
        {
            try
            {
                var userEntity = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
                return userEntity ?? null!;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while getting a user by ID: {ex.Message}");
                return null!;
            }
        }

        // Update
        public async Task<UserEntity> UpdateUserAsync(UserEntity userEntity)
        {
            try
            {
                _context.Users.Update(userEntity);
                await _context.SaveChangesAsync();
                return userEntity;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Database update error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating a user: {ex.Message}");
            }
            return null!;
        }

        // Delete
        public async Task<UserEntity> DeleteUserAsync(int id)
        {
            try
            {
                var userEntity =
                    await _context.Users.FirstOrDefaultAsync(x => x.Id == id)
                    ?? throw new InvalidOperationException("User not found.");
                _context.Users.Remove(userEntity);
                await _context.SaveChangesAsync();
                return userEntity;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting a user: {ex.Message}");
                return null!;
            }
        }
    }
}
