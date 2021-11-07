// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FindUserByUsernameOrEmailRequestMessageConsumer.cs" company="Stone Assemblies">
//   Copyright © 2021 - 2021 Stone Assemblies. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace StoneAssemblies.Keycloak.Consumers
{
    using System;
    using System.Threading.Tasks;

    using MassTransit;

    using Serilog;

    using StoneAssemblies.Keycloak.Messages;
    using StoneAssemblies.Keycloak.Models;
    using StoneAssemblies.Keycloak.Services.Interfaces;

    /// <summary>
    ///     The find user by username or email request message consumer.
    /// </summary>
    public class FindUserByUsernameOrEmailRequestMessageConsumer : KeycloakMessagesConsumerBase,
                                                                   IConsumer<FindUserByUsernameOrEmailRequestMessage>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="FindUserByUsernameOrEmailRequestMessageConsumer" /> class.
        /// </summary>
        /// <param name="userRepository">
        ///     The user repository.
        /// </param>
        /// <param name="encryptionService">
        ///     The encryption service.
        /// </param>
        public FindUserByUsernameOrEmailRequestMessageConsumer(IUserRepository userRepository, IEncryptionService encryptionService)
            : base(userRepository, encryptionService)
        {
        }

        /// <summary>
        ///     The consume.
        /// </summary>
        /// <param name="context">
        ///     The context.
        /// </param>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        async Task IConsumer<FindUserByUsernameOrEmailRequestMessage>.Consume(
            ConsumeContext<FindUserByUsernameOrEmailRequestMessage> context)
        {
            Log.Information("Finding user by username or email '{UsernameOrEmail}'", context.Message.UsernameOrEmail);

            User user = null;
            try
            {
                user = await this.UserRepository.FindUserByUsernameOrEmailAsync(context.Message.UsernameOrEmail);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Error finding user by Username Or Email '{UsernameOrEmail}'", context.Message.UsernameOrEmail);
            }

            await context.RespondAsync(
                new FindUserByUsernameOrEmailResponseMessage
                    {
                        CorrelationId = context.Message.CorrelationId,
                        User = user,
                    });
        }
    }

    /// <summary>
    ///     The find user by username or email request message consumer.
    /// </summary>
    /// <typeparam name="TUserRepository">
    ///     The repository type.
    /// </typeparam>
    public class FindUserByUsernameOrEmailRequestMessageConsumer<TUserRepository> : FindUserByUsernameOrEmailRequestMessageConsumer
        where TUserRepository : IUserRepository
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="FindUserByUsernameOrEmailRequestMessageConsumer{TUserRepository}" /> class.
        /// </summary>
        /// <param name="userRepository">
        ///     The user repository.
        /// </param>
        /// <param name="encryptionService">
        ///     The encryption service.
        /// </param>
        public FindUserByUsernameOrEmailRequestMessageConsumer(TUserRepository userRepository, IEncryptionService encryptionService)
            : base(userRepository, encryptionService)
        {
        }
    }
}