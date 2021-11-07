// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceCollectionConfiguratorExtensions.cs" company="Stone Assemblies">
//   Copyright © 2021 - 2021 Stone Assemblies. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace StoneAssemblies.Keycloak.Extensions
{
    using MassTransit.ExtensionsDependencyInjectionIntegration;

    using StoneAssemblies.Keycloak.Consumers;
    using StoneAssemblies.Keycloak.Services.Interfaces;

    /// <summary>
    ///     The service collection configurator extensions.
    /// </summary>
    public static class ServiceCollectionBusConfiguratorExtensions
    {
        /// <summary>
        ///     The add keycloak consumers.
        /// </summary>
        /// <param name="serviceCollectionBusConfigurator">
        ///     The service collection bus configurator.
        /// </param>
        /// <typeparam name="TUserRepository">
        ///     The user repository.
        /// </typeparam>
        public static void AddKeycloakConsumers<TUserRepository>(
            this IServiceCollectionBusConfigurator serviceCollectionBusConfigurator)
            where TUserRepository : IUserRepository
        {
            serviceCollectionBusConfigurator.AddConsumer<UsersCountRequestMessageConsumer<TUserRepository>>();
            serviceCollectionBusConfigurator.AddConsumer<FindUserByIdRequestMessageConsumer<TUserRepository>>();
            serviceCollectionBusConfigurator.AddConsumer<FindUserByUsernameOrEmailRequestMessageConsumer<TUserRepository>>();
            serviceCollectionBusConfigurator.AddConsumer<UsersRequestMessageConsumer<TUserRepository>>();
            serviceCollectionBusConfigurator.AddConsumer<ValidateCredentialsRequestMessageConsumer<TUserRepository>>();
            serviceCollectionBusConfigurator.AddConsumer<UpdateCredentialsRequestMessageConsumer<TUserRepository>>();
        }
    }
}