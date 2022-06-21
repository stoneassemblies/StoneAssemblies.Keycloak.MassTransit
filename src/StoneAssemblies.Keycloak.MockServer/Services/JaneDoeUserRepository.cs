namespace StoneAssemblies.Keycloak.MockServer.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using StoneAssemblies.Keycloak.Models;
    using StoneAssemblies.Keycloak.Services.Interfaces;

    /// <summary>
    ///     The mock user repository.
    /// </summary>
    public class JaneDoeUserRepository : IUserRepository
    {
        /// <summary>
        ///     The passwords.
        /// </summary>
        private static readonly Dictionary<Guid, string> Passwords = new Dictionary<Guid, string>();

        /// <summary>
        ///     The users.
        /// </summary>
        private static readonly Dictionary<Guid, User> Users = new Dictionary<Guid, User>();

        /// <summary>
        ///     Initializes static members of the <see cref="JaneDoeUserRepository" /> class.
        /// </summary>
        static JaneDoeUserRepository()
        {
            var rng = new Random();
            var rolesList = new List<string>() { "User", "Moderator", "Admin", "Client_Bicsa", "Client_Bpa" };
            for (var i = 0; i < 1000; i++)
            {
                var rolesSeparateComma = "";
                var cant = rng.Next(0, rolesList.Count);
                for (var j = 0; j < cant; j++)
                {
                    rolesSeparateComma += rolesList.ElementAt(j);
                    if (j < cant - 1)
                    {
                        rolesSeparateComma += ",";
                    }
                }
                var guid = Guid.NewGuid();
                Users[guid] = new User
                                  {
                                      Id = guid.ToString(),
                                      FirstName = "Jane",
                                      LastName = "Doe",
                                      Username = $"jane.doe{i}",
                                      Email = $"jane.doe{i}@domain.cu",
                                      Enabled = true,
                                      Roles = rolesSeparateComma,
                                  };
                Passwords[guid] = "Password123!";
            }
        }

        /// <inheritdoc />
        public Task<User> FindUserByIdAsync(string userId)
        {
            if (Guid.TryParse(userId, out var guid) && Users.TryGetValue(guid, out var user))
            {
                return Task.FromResult(user);
            }

            return Task.FromResult<User>(null);
        }

        /// <inheritdoc />
        public Task<User> FindUserByUsernameOrEmailAsync(string usernameOrEmail)
        {
            var user = Users.Values.FirstOrDefault(u => u.Username == usernameOrEmail || u.Email == usernameOrEmail);
            return Task.FromResult(user);
        }

        /// <inheritdoc />
        public Task<bool> UpdateCredentialsAsync(string username, string password)
        {
            var user = Users.Values.FirstOrDefault(u => u.Username == username);
            if (user != null)
            {
                Passwords[Guid.Parse(user.Id)] = password;
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        /// <inheritdoc />
        public IAsyncEnumerable<User> UsersAsync(int offset, int take)
        {
            return Users.Values.Skip(offset).Take(take).ToAsyncEnumerable();
        }

        /// <inheritdoc />
        public Task<int> UsersCountAsync()
        {
            return Task.FromResult(Users.Count);
        }

        /// <inheritdoc />
        public Task<bool> ValidateCredentialsAsync(string username, string password)
        {
            var user = Users.Values.FirstOrDefault(u => u.Username == username);

            return Task.FromResult(user != null && Passwords[Guid.Parse(user.Id)] == password);
        }
    }
}