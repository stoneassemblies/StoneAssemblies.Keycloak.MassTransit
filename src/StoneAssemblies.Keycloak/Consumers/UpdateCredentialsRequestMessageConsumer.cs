// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateCredentialsRequestMessageConsumer.cs" company="Stone Assemblies">
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
    ///     The update credentials request message consumer.
    /// </summary>
    public class UpdateCredentialsRequestMessageConsumer : KeycloakMessagesConsumerBase, IConsumer<UpdateCredentialsRequestMessage>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="UpdateCredentialsRequestMessageConsumer" /> class.
        /// </summary>
        /// <param name="userRepository">
        ///     The user repository.
        /// </param>
        /// <param name="encryptionService">
        ///     The encryption service.
        /// </param>
        public UpdateCredentialsRequestMessageConsumer(IUserRepository userRepository, IEncryptionService encryptionService)
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
        async Task IConsumer<UpdateCredentialsRequestMessage>.Consume(ConsumeContext<UpdateCredentialsRequestMessage> context)
        {
            Log.Information("Updating credentials of {Username}", context.Message.Username);

            var password = this.EncryptionService.Decrypt(context.Message.Password);

            var succeeded = false;
            try
            {
                succeeded = await this.UserRepository.UpdateCredentialsAsync(context.Message.Username, password);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Error updating credentials of user {UserName}", context.Message.Username);
            }

            await context.RespondAsync(
                new UpdateCredentialsResponseMessage
                    {
                        CorrelationId = context.Message.CorrelationId,
                        Succeeded = succeeded,
                    });
        }
    }

    /// <summary>
    ///     The update credentials request message consumer.
    /// </summary>
    /// <typeparam name="TUserRepository">
    ///     The repository type.
    /// </typeparam>
    public class UpdateCredentialsRequestMessageConsumer<TUserRepository> : UpdateCredentialsRequestMessageConsumer
        where TUserRepository : IUserRepository
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="UpdateCredentialsRequestMessageConsumer{TUserRepository}" /> class.
        /// </summary>
        /// <param name="userRepository">
        ///     The user repository.
        /// </param>
        /// <param name="encryptionService">
        ///     The encryption service.
        /// </param>
        public UpdateCredentialsRequestMessageConsumer(TUserRepository userRepository, IEncryptionService encryptionService)
            : base(userRepository, encryptionService)
        {
        }
    }
}