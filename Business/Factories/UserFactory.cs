using Business.Models;
using Data.Contexts;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Business.Factories
{
    public class UserFactory(DataContext context)
    {
        private readonly DataContext _context =
            context ?? throw new ArgumentNullException(nameof(context));

        public async Task<UserEntity> CreateAsync(UserRegistrationForm form)
        {
            bool emailExists = await _context.Users.AnyAsync(u => u.Email == form.Email);
            if (emailExists)
            {
                throw new InvalidOperationException("Email is already in use.");
            }

            return new UserEntity
            {
                FirstName = form.FirstName,
                LastName = form.LastName,
                Email = form.Email,
                PhoneNumber = form.PhoneNumber,
            };
        }

        public static User Create(UserEntity entity)
        {
            return new User
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber,
            };
        }
    }
}
