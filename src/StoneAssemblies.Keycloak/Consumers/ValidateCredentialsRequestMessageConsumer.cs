// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidateCredentialsRequestMessageConsumer.cs" company="Stone Assemblies">
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
    ///     The validate credentials request message consumer.
    /// </summary>
    public class ValidateCredentialsRequestMessageConsumer : KeycloakMessagesConsumerBase, IConsumer<ValidateCredentialsRequestMessage>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ValidateCredentialsRequestMessageConsumer" /> class.
        /// </summary>
        /// <param name="userRepository">
        ///     The user repository.
        /// </param>
        /// <param name="encryptionService">
        ///     The encryption service.
        /// </param>
        public ValidateCredentialsRequestMessageConsumer(IUserRepository userRepository, IEncryptionService encryptionService)
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
        async Task IConsumer<ValidateCredentialsRequestMessage>.Consume(ConsumeContext<ValidateCredentialsRequestMessage> context)
        {
            Log.Information("Validating user credentials {Username}", context.Message.Username);

            var password = this.EncryptionService.Decrypt(context.Message.Password);

            var succeeded = false;
            try
            {
                succeeded = await this.UserRepository.ValidateCredentialsAsync(context.Message.Username, password);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Error validating credential of user {Username}", context.Message.Username);
            }

            await context.RespondAsync(
                new ValidateCredentialsResponseMessage
                    {
                        CorrelationId = context.Message.CorrelationId,
                        Succeeded = succeeded,
                    });
        }
    }

    /// <summary>
    ///     The validate credentials request message consumer.
    /// </summary>
    /// <typeparam name="TUserRepository">
    ///     The user repository.
    /// </typeparam>
    public class ValidateCredentialsRequestMessageConsumer<TUserRepository> : ValidateCredentialsRequestMessageConsumer
        where TUserRepository : IUserRepository
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ValidateCredentialsRequestMessageConsumer{TUserRepository}" /> class.
        /// </summary>
        /// <param name="userRepository">
        ///     The user repository.
        /// </param>
        /// <param name="encryptionService">
        ///     The encryption service.
        /// </param>
        public ValidateCredentialsRequestMessageConsumer(TUserRepository userRepository, IEncryptionService encryptionService)
            : base(userRepository, encryptionService)
        {
        }
    }
}