// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UsersRequestMessageConsumer.cs" company="Stone Assemblies">
//   Copyright © 2021 - 2021 Stone Assemblies. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace StoneAssemblies.Keycloak.Consumers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MassTransit;

    using Serilog;

    using StoneAssemblies.Keycloak.Messages;
    using StoneAssemblies.Keycloak.Models;
    using StoneAssemblies.Keycloak.Services.Interfaces;

    /// <summary>
    ///     The users request message consumer.
    /// </summary>
    public class UsersRequestMessageConsumer : KeycloakMessagesConsumerBase, IConsumer<UsersRequestMessage>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="UsersRequestMessageConsumer" /> class.
        /// </summary>
        /// <param name="userRepository">
        ///     The user repository.
        /// </param>
        /// <param name="encryptionService">
        ///     The encryption service.
        /// </param>
        public UsersRequestMessageConsumer(IUserRepository userRepository, IEncryptionService encryptionService)
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
        async Task IConsumer<UsersRequestMessage>.Consume(ConsumeContext<UsersRequestMessage> context)
        {
            Log.Information("Listing users from '{Offset}' taking '{Take}'", context.Message.Offset, context.Message.Take);

            List<User> users = null;
            try
            {
                users = await this.UserRepository.UsersAsync(context.Message.Offset, context.Message.Take).ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Error listing users");
            }

            await context.RespondAsync(
                new UsersResponseMessage
                    {
                        CorrelationId = context.Message.CorrelationId,
                        Users = users,
                    });
        }
    }

    /// <summary>
    ///     The users request message consumer.
    /// </summary>
    /// <typeparam name="TUserRepository">
    ///     The user repository.
    /// </typeparam>
    public class UsersRequestMessageConsumer<TUserRepository> : UsersRequestMessageConsumer
        where TUserRepository : IUserRepository
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="UsersRequestMessageConsumer{TUserRepository}" /> class.
        /// </summary>
        /// <param name="userRepository">
        ///     The user repository.
        /// </param>
        /// <param name="encryptionService">
        ///     The encryption service.
        /// </param>
        public UsersRequestMessageConsumer(TUserRepository userRepository, IEncryptionService encryptionService)
            : base(userRepository, encryptionService)
        {
        }
    }
}