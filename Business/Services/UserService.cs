using Business.Interfaces;
using Data.Entities;
using Data.Repositories;

namespace Business.Services
{
    public class UserService(UserRepository userRepository) : IUserService
    {
        private readonly UserRepository _userRepository = userRepository;

        // Create
        public async Task<UserEntity?> CreateUserAsync(UserEntity userEntity)
        {
            return await _userRepository.AddUserAsync(userEntity);
        }

        // Read
        public async Task<IEnumerable<UserEntity>> GetUsersAsync()
        {
            return await _userRepository.GetUsersAsync();
        }

        public async Task<UserEntity?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        // Update
        public async Task<UserEntity?> UpdateUserAsync(UserEntity userEntity)
        {
            return await _userRepository.UpdateAsync(userEntity);
        }

        // Delete
        public async Task<UserEntity?> DeleteUserAsync(int id)
        {
            return await _userRepository.DeleteAsync(id);
        }
    }
}
