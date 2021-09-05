// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FindUserByUsernameOrEmailRequestMessage.cs" company="Stone Assemblies">
//   Copyright © 2021 - 2021 Stone Assemblies. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace StoneAssemblies.Keycloak.Messages
{
    /// <summary>
    ///     The find user by username or email request message.
    /// </summary>
    public class FindUserByUsernameOrEmailRequestMessage : KeycloakMessage
    {
        /// <summary>
        ///     Gets or sets the username or email.
        /// </summary>
        public string UsernameOrEmail { get; set; }
    }
}