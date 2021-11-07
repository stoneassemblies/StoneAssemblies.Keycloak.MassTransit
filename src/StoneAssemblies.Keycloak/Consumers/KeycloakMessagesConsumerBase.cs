// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeycloakMessagesConsumerBase.cs" company="Stone Assemblies">
//   Copyright © 2021 - 2021 Stone Assemblies. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace StoneAssemblies.Keycloak.Consumers
{
    using StoneAssemblies.Keycloak.Services.Interfaces;

    /// <summary>
    ///     The keycloak messages consumer.
    /// </summary>
    public abstract class KeycloakMessagesConsumerBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="KeycloakMessagesConsumerBase" /> class.
        /// </summary>
        /// <param name="userRepository">
        ///     The user repository.
        /// </param>
        public KeycloakMessagesConsumerBase(IUserRepository userRepository, IEncryptionService encryptionService)
        {
            this.UserRepository = userRepository;
            this.EncryptionService = encryptionService;
        }

        /// <summary>
        ///     The encryption service.
        /// </summary>
        protected IEncryptionService EncryptionService { get; }

        /// <summary>
        ///     The user repository.
        /// </summary>
        protected IUserRepository UserRepository { get; }
    }
}