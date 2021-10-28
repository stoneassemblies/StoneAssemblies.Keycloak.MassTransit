// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeycloakMessagesConsumer.cs" company="Stone Assemblies">
//   Copyright © 2021 - 2021 Stone Assemblies. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace StoneAssemblies.Keycloak
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
    ///     The keycloak messages consumer.
    /// </summary>
    public class KeycloakMessagesConsumer : IConsumer<UsersCountRequestMessage>,
                                            IConsumer<FindUserByIdRequestMessage>,
                                            IConsumer<FindUserByUsernameOrEmailRequestMessage>,
                                            IConsumer<UsersRequestMessage>,
                                            IConsumer<ValidateCredentialsRequestMessage>,
                                            IConsumer<UpdateCredentialsRequestMessage>
    {
        /// <summary>
        /// The encryption service.
        /// </summary>
        private readonly IEncryptionService encryptionService;

        /// <summary>
        ///     The user repository.
        /// </summary>
        private readonly IUserRepository userRepository;

        /// <summary>
        ///     Initializes a new instance of the <see cref="KeycloakMessagesConsumer" /> class.
        /// </summary>
        /// <param name="userRepository">
        ///     The user repository.
        /// </param>
        public KeycloakMessagesConsumer(IUserRepository userRepository, IEncryptionService encryptionService)
        {
            this.userRepository = userRepository;
            this.encryptionService = encryptionService;
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
        async Task IConsumer<FindUserByIdRequestMessage>.Consume(ConsumeContext<FindUserByIdRequestMessage> context)
        {
            User user = null;
            try
            {
                user = await this.userRepository.FindUserByIdAsync(context.Message.UserId);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Error finding user by Id {UserId}", context.Message.UserId);
            }

            await context.RespondAsync(
                new FindUserByIdResponseMessage
                    {
                        CorrelationId = context.Message.CorrelationId,
                        User = user
                    });
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
            User user = null;
            try
            {
                user = await this.userRepository.FindUserByUsernameOrEmailAsync(context.Message.UsernameOrEmail);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Error finding user by Username Or Email {UsernameOrEmail}", context.Message.UsernameOrEmail);
            }

            await context.RespondAsync(
                new FindUserByUsernameOrEmailResponseMessage
                    {
                        CorrelationId = context.Message.CorrelationId,
                        User = user
                });
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
            var password = this.encryptionService.Decrypt(context.Message.Password);

            var succeeded = false;
            try
            {
                succeeded = await this.userRepository.UpdateCredentialsAsync(context.Message.Username, password);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Error updating credentials of user {UserName}", context.Message.Username);
            }

            await context.RespondAsync(
                new UpdateCredentialsResponseMessage
                    {
                        CorrelationId = context.Message.CorrelationId,
                        Succeeded = succeeded
                });
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
            var count = 0;
            try
            {
                count = await this.userRepository.UsersCountAsync();
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Error counting users");
            }

            await context.RespondAsync(
                new UsersCountResponseMessage
                    {
                        CorrelationId = context.Message.CorrelationId,
                        Count = count
                    });
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
            List<User> users = null;
            try
            {
                users = await this.userRepository.UsersAsync(context.Message.Offset, context.Message.Take).ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Error listing users");
            }

            await context.RespondAsync(
                new UsersResponseMessage
                    {
                        CorrelationId = context.Message.CorrelationId,
                        Users = users
                    });
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
            var password = this.encryptionService.Decrypt(context.Message.Password);

            bool succeeded = false;
            try
            {
                succeeded = await this.userRepository.ValidateCredentialsAsync(context.Message.Username, password);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Error validating credential of user {Username}", context.Message.Username);
            }

            await context.RespondAsync(
                new ValidateCredentialsResponseMessage
                    {
                        CorrelationId = context.Message.CorrelationId,
                        Succeeded = succeeded
                });
        }
    }

    /// <summary>
    /// The keycloak messages consumer.
    /// </summary>
    /// <typeparam name="TUserRepository">
    /// The user repository type.
    /// </typeparam>
    public class KeycloakMessagesConsumer<TUserRepository> : KeycloakMessagesConsumer where TUserRepository : IUserRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeycloakMessagesConsumer{TUserRepository}"/> class.
        /// </summary>
        /// <param name="userRepository">
        /// The user repository.
        /// </param>
        /// <param name="encryptionService">
        /// The encryption service.
        /// </param>
        public KeycloakMessagesConsumer(TUserRepository userRepository, IEncryptionService encryptionService)
            : base(userRepository, encryptionService)
        {
        }
    }
}