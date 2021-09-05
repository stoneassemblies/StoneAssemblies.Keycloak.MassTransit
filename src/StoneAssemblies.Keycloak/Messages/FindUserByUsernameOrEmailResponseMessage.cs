// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FindUserByUsernameOrEmailResponseMessage.cs" company="Stone Assemblies">
//   Copyright © 2021 - 2021 Stone Assemblies. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace StoneAssemblies.Keycloak.Messages
{
    using StoneAssemblies.Keycloak.Models;

    /// <summary>
    ///     The find user by username or email response message.
    /// </summary>
    public class FindUserByUsernameOrEmailResponseMessage : KeycloakMessage
    {
        /// <summary>
        ///     Gets or sets the user.
        /// </summary>
        public User User { get; set; }
    }
}