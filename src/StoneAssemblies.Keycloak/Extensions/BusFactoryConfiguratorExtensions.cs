// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BusFactoryConfiguratorExtensions.cs" company="Stone Assemblies">
//   Copyright © 2021 - 2021 Stone Assemblies. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace StoneAssemblies.Keycloak.Extensions
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using MassTransit;

    using StoneAssemblies.Keycloak.Consumers;
    using StoneAssemblies.Keycloak.Messages;
    using StoneAssemblies.Keycloak.Services.Interfaces;

    /// <summary>
    ///     The bus factory configurator extensions.
    /// </summary>
    public static class BusFactoryConfiguratorExtensions
    {
        /// <summary>
        ///     The keycloak receive endpoints.
        /// </summary>
        /// <param name="configurator">
        ///     The configurator.
        /// </param>
        /// <param name="context">
        ///     The context.
        /// </param>
        /// <param name="configure">
        ///     The configure.
        /// </param>
        /// <typeparam name="TUserRepository">
        ///     The user repository type.
        /// </typeparam>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public static void KeycloakReceiveEndpoints<TUserRepository>(
            this IBusFactoryConfigurator configurator, IBusRegistrationContext context, Action<IReceiveEndpointConfigurator> configure)
            where TUserRepository : IUserRepository
        {
            configurator.ReceiveEndpoint(
                nameof(UsersCountRequestMessage),
                e =>
                    {
                        configure(e);
                        e.ConfigureConsumer<UsersCountRequestMessageConsumer<TUserRepository>>(context);
                    });

            configurator.ReceiveEndpoint(
                nameof(FindUserByIdRequestMessage),
                e =>
                    {
                        configure(e);
                        e.ConfigureConsumer<FindUserByIdRequestMessageConsumer<TUserRepository>>(context);
                    });

            configurator.ReceiveEndpoint(
                nameof(FindUserByUsernameOrEmailRequestMessage),
                e =>
                    {
                        configure(e);
                        e.ConfigureConsumer<FindUserByUsernameOrEmailRequestMessageConsumer<TUserRepository>>(context);
                    });

            configurator.ReceiveEndpoint(
                nameof(UsersRequestMessage),
                e =>
                    {
                        configure(e);
                        e.ConfigureConsumer<UsersRequestMessageConsumer<TUserRepository>>(context);
                    });

            configurator.ReceiveEndpoint(
                nameof(ValidateCredentialsRequestMessage),
                e =>
                    {
                        configure(e);
                        e.ConfigureConsumer<ValidateCredentialsRequestMessageConsumer<TUserRepository>>(context);
                    });

            configurator.ReceiveEndpoint(
                nameof(UpdateCredentialsRequestMessage),
                e =>
                    {
                        configure(e);
                        e.ConfigureConsumer<UpdateCredentialsRequestMessageConsumer<TUserRepository>>(context);
                    });
        }
    }
}