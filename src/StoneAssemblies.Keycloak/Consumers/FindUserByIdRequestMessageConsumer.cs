namespace StoneAssemblies.Keycloak
{
    using System;
    using System.Threading.Tasks;

    using MassTransit;

    using Serilog;

    using StoneAssemblies.Keycloak.Consumers;
    using StoneAssemblies.Keycloak.Messages;
    using StoneAssemblies.Keycloak.Models;
    using StoneAssemblies.Keycloak.Services.Interfaces;

    public class FindUserByIdRequestMessageConsumer : KeycloakMessagesConsumerBase, IConsumer<FindUserByIdRequestMessage>
    {
        public FindUserByIdRequestMessageConsumer(IUserRepository userRepository, IEncryptionService encryptionService)
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
        async Task IConsumer<FindUserByIdRequestMessage>.Consume(ConsumeContext<FindUserByIdRequestMessage> context)
        {
            Log.Information("Finding user by ID {UserId}", context.Message.UserId);

            User user = null;
            try
            {
                user = await this.UserRepository.FindUserByIdAsync(context.Message.UserId);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Error finding user by Id {UserId}", context.Message.UserId);
            }

            await context.RespondAsync(
                new FindUserByIdResponseMessage
                    {
                        CorrelationId = context.Message.CorrelationId,
                        User = user,
                    });
        }
    }

    /// <summary>
    ///     The find user by id request message consumer.
    /// </summary>
    /// <typeparam name="TUserRepository">
    ///     The repository type.
    /// </typeparam>
    public class FindUserByIdRequestMessageConsumer<TUserRepository> : FindUserByIdRequestMessageConsumer
        where TUserRepository : IUserRepository
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="KeycloakMessagesConsumerBase{TUserRepository}" /> class.
        /// </summary>
        /// <param name="userRepository">
        ///     The user repository.
        /// </param>
        /// <param name="encryptionService">
        ///     The encryption service.
        /// </param>
        public FindUserByIdRequestMessageConsumer(TUserRepository userRepository, IEncryptionService encryptionService)
            : base(userRepository, encryptionService)
        {
        }
    }
}