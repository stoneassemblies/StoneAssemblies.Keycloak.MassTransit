// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeycloakMessagesConsumer.cs" company="Stone Assemblies">
//   Copyright © 2021 - 2021 Stone Assemblies. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace StoneAssemblies.Keycloak
{
    using System.Threading.Tasks;

    using MassTransit;

    using StoneAssemblies.Keycloak.Messages;
    using StoneAssemblies.Keycloak.Services.Interfaces;

    /// <summary>
    /// The keycloak messages consumer.
    /// </summary>
    public class KeycloakMessagesConsumer : IConsumer<UsersCountRequestMessage>,
                                            IConsumer<FindUserByIdRequestMessage>,
                                            IConsumer<FindUserByUsernameOrEmailRequestMessage>,
                                            IConsumer<UsersRequestMessage>,
                                            IConsumer<ValidateCredentialsRequestMessage>,
                                            IConsumer<UpdateCredentialsRequestMessage>
    {
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
        public KeycloakMessagesConsumer(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        /// <summary>
        /// The consume.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        async Task IConsumer<FindUserByIdRequestMessage>.Consume(ConsumeContext<FindUserByIdRequestMessage> context)
        {
            await context.RespondAsync(
                new FindUserByIdResponseMessage
                    {
                        CorrelationId = context.Message.CorrelationId,
                        User = await this.userRepository.FindUserByIdAsync(context.Message.UserId)
                    });
        }

        /// <summary>
        /// The consume.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        async Task IConsumer<FindUserByUsernameOrEmailRequestMessage>.Consume(
            ConsumeContext<FindUserByUsernameOrEmailRequestMessage> context)
        {
            await context.RespondAsync(
                new FindUserByUsernameOrEmailResponseMessage
                    {
                        CorrelationId = context.Message.CorrelationId,
                        User = await this.userRepository.FindUserByUsernameOrEmailAsync(context.Message.UsernameOrEmail)
                    });
        }

        /// <summary>
        /// The consume.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        async Task IConsumer<UpdateCredentialsRequestMessage>.Consume(ConsumeContext<UpdateCredentialsRequestMessage> context)
        {
            await context.RespondAsync(
                new UpdateCredentialsResponseMessage
                    {
                        CorrelationId = context.Message.CorrelationId,
                        Succeeded = await this.userRepository.UpdateCredentialsAsync(
                                        context.Message.Username,
                                        context.Message.Password)
                    });
        }

        /// <summary>
        /// The consume.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        async Task IConsumer<UsersCountRequestMessage>.Consume(ConsumeContext<UsersCountRequestMessage> context)
        {
            await context.RespondAsync(
                new UsersCountResponseMessage
                    {
                        CorrelationId = context.Message.CorrelationId,
                        Count = await this.userRepository.UsersCountAsync()
                    });
        }

        /// <summary>
        /// The consume.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        async Task IConsumer<UsersRequestMessage>.Consume(ConsumeContext<UsersRequestMessage> context)
        {
            await context.RespondAsync(
                new UsersResponseMessage
                    {
                        CorrelationId = context.Message.CorrelationId,
                        Users = await this.userRepository.UsersAsync(context.Message.Offset, context.Message.Take)
                    });
        }

        /// <summary>
        /// The consume.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        async Task IConsumer<ValidateCredentialsRequestMessage>.Consume(ConsumeContext<ValidateCredentialsRequestMessage> context)
        {
            await context.RespondAsync(
                new ValidateCredentialsResponseMessage
                    {
                        CorrelationId = context.Message.CorrelationId,
                        Succeeded = await this.userRepository.ValidateCredentialsAsync(
                                        context.Message.Username,
                                        context.Message.Password)
                    });
        }
    }
}