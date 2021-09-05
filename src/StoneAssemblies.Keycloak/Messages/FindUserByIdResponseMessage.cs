// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FindUserByIdResponseMessage.cs" company="Stone Assemblies">
//   Copyright © 2021 - 2021 Stone Assemblies. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace StoneAssemblies.Keycloak.Messages
{
    using StoneAssemblies.Keycloak.Models;

    /// <summary>
    ///     The find user by id response message.
    /// </summary>
    public class FindUserByIdResponseMessage : KeycloakMessage
    {
        /// <summary>
        ///     Gets or sets the user.
        /// </summary>
        public User User { get; set; }
    }
}