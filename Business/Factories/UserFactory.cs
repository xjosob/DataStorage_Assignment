﻿using Business.Models;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Business.Factories
{
    public class UserFactory(DbContext context)
    {
        private readonly DbContext _context =
            context ?? throw new ArgumentNullException(nameof(context));

        public async Task<UserEntity> Create(UserRegistrationForm form)
        {
            #region // Validation generated by ChatGPT. Checks if email is already in use.
            bool emailExists = await _context
                .Set<UserEntity>()
                .AnyAsync(u => u.Email == form.Email);
            if (emailExists)
            {
                throw new InvalidOperationException("Email is already in use.");
            }
            #endregion

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
