// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockUserRepository.cs" company="Stone Assemblies">
//   Copyright © 2021 - 2021 Stone Assemblies. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace StoneAssemblies.Keycloak.MockServer.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using StoneAssemblies.Keycloak.Models;
    using StoneAssemblies.Keycloak.Services.Interfaces;

    /// <summary>
    ///     The mock user repository.
    /// </summary>
    public class MockUserRepository : IUserRepository
    {
        /// <inheritdoc />
        public async Task<User> FindUserByIdAsync(string userId)
        {
            return new User
                       {
                           Id = userId,
                           FirstName = "Igr Alexánder",
                           LastName = "Fernández Saúco",
                           Username = "alexfdezsauco",
                           Email = "alexfdezsauco@domain.com",
                           Enabled = true
                       };
        }

        /// <inheritdoc />
        public async Task<User> FindUserByUsernameOrEmailAsync(string usernameOrEmail)
        {
            return new User
                       {
                           Id = Guid.NewGuid().ToString(),
                           FirstName = "Igr Alexánder",
                           LastName = "Fernández Saúco",
                           Username = "alexfdezsauco",
                           Email = usernameOrEmail,
                           Enabled = true
                       };
        }

        /// <inheritdoc />
        public async Task<bool> UpdateCredentialsAsync(string username, string password)
        {
            return true;
        }

        /// <inheritdoc />
        public async Task<List<User>> UsersAsync(int offset, int take)
        {
            var usersCount = await this.UsersCountAsync();
            var range = offset + take;
            var count = Math.Min(usersCount, range);

            var users = new List<User>();

            for (var i = offset; i < count; i++)
            {
                users.Add(
                    new User
                        {
                            Id = Guid.NewGuid().ToString(),
                            FirstName = "Igr Alexánder",
                            LastName = "Fernández Saúco",
                            Username = $"alexfdezsauco{i}",
                            Email = $"alexfdezsauco{i}@domain.cu",
                            Enabled = true
                        });
            }

            return users;
        }

        /// <inheritdoc />
        public async Task<int> UsersCountAsync()
        {
            return 50;
        }

        /// <inheritdoc />
        public async Task<bool> ValidateCredentialsAsync(string username, string password)
        {
            return true;
        }
    }
}