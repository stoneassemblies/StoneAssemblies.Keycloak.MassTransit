// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUserRepository.cs" company="Stone Assemblies">
//   Copyright © 2021 - 2021 Stone Assemblies. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace StoneAssemblies.Keycloak.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using StoneAssemblies.Keycloak.Models;

    /// <summary>
    ///     The UserRepository interface.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        ///     Finds user by id async.
        /// </summary>
        /// <param name="userId">
        ///     The message user id.
        /// </param>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        Task<User> FindUserByIdAsync(string userId);

        /// <summary>
        ///     Finds user by username or email async.
        /// </summary>
        /// <param name="usernameOrEmail">
        ///     The username or email.
        /// </param>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        Task<User> FindUserByUsernameOrEmailAsync(string usernameOrEmail);

        /// <summary>
        ///     Updates credentials async.
        /// </summary>
        /// <param name="username">
        ///     The username.
        /// </param>
        /// <param name="password">
        ///     The password.
        /// </param>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        Task<bool> UpdateCredentialsAsync(string username, string password);

        /// <summary>
        ///     Users async.
        /// </summary>
        /// <param name="offset">
        ///     The offset.
        /// </param>
        /// <param name="take">
        ///     The take.
        /// </param>
        /// <returns>
        ///     The async enumeration  of <see cref="User" />.
        /// </returns>
        IAsyncEnumerable<User> UsersAsync(int offset, int take);

        /// <summary>
        ///     Users count async.
        /// </summary>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        Task<int> UsersCountAsync();

        /// <summary>
        ///     Validates credentials async.
        /// </summary>
        /// <param name="username">
        ///     The username.
        /// </param>
        /// <param name="password">
        ///     The password.
        /// </param>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        Task<bool> ValidateCredentialsAsync(string username, string password);
    }
}