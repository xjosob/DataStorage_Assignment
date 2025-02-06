using Business.Interfaces;
using Data.Contexts;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public class UserService(DataContext context) : IUserService
    {
        private readonly DataContext _context = context;

        public async Task<UserEntity> CreateUser(UserEntity userEntity)
        {
            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();
            return userEntity;
        }

        public async Task<IEnumerable<UserEntity>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<UserEntity> GetUserByIdAsync(int id)
        {
            var userEntity = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            return userEntity ?? null!;
        }

        public async Task<UserEntity> UpdateUserAsync(UserEntity userEntity)
        {
            _context.Users.Update(userEntity);
            await _context.SaveChangesAsync();
            return userEntity;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var userEntity = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (userEntity == null)
            {
                return false;
            }
            _context.Users.Remove(userEntity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
