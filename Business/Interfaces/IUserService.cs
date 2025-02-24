using Data.Entities;

namespace Business.Interfaces
{
    public interface IUserService
    {
        Task<UserEntity?> CreateUserAsync(UserEntity userEntity);
        Task<IEnumerable<UserEntity>> GetUsersAsync();
        Task<UserEntity?> GetUserByIdAsync(int id);
        Task<UserEntity?> UpdateUserAsync(UserEntity userEntity);
        Task<UserEntity?> DeleteUserAsync(int id);
    }
}
