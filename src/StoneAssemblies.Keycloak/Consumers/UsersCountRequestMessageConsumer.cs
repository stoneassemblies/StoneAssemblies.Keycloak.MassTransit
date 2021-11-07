// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UsersCountRequestMessageConsumer.cs" company="Stone Assemblies">
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
    using StoneAssemblies.Keycloak.Services.Interfaces;

    /// <summary>
    ///     The users count request message consumer.
    /// </summary>
    public class UsersCountRequestMessageConsumer : KeycloakMessagesConsumerBase, IConsumer<UsersCountRequestMessage>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="UsersCountRequestMessageConsumer" /> class.
        /// </summary>
        /// <param name="userRepository">
        ///     The user repository.
        /// </param>
        /// <param name="encryptionService">
        ///     The encryption service.
        /// </param>
        public UsersCountRequestMessageConsumer(IUserRepository userRepository, IEncryptionService encryptionService)
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
        async Task IConsumer<UsersCountRequestMessage>.Consume(ConsumeContext<UsersCountRequestMessage> context)
        {
            Log.Information("Counting users");

            var count = 0;
            try
            {
                count = await this.UserRepository.UsersCountAsync();
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Error counting users");
            }

            await context.RespondAsync(
                new UsersCountResponseMessage
                    {
                        CorrelationId = context.Message.CorrelationId,
                        Count = count,
                    });
        }
    }

    /// <summary>
    ///     The users count request message consumer.
    /// </summary>
    /// <typeparam name="TUserRepository">
    ///     The repository type.
    /// </typeparam>
    public class UsersCountRequestMessageConsumer<TUserRepository> : UsersCountRequestMessageConsumer
        where TUserRepository : IUserRepository
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="UsersCountRequestMessageConsumer{TUserRepository}" /> class.
        /// </summary>
        /// <param name="userRepository">
        ///     The user repository.
        /// </param>
        /// <param name="encryptionService">
        ///     The encryption service.
        /// </param>
        public UsersCountRequestMessageConsumer(TUserRepository userRepository, IEncryptionService encryptionService)
            : base(userRepository, encryptionService)
        {
        }
    }
}