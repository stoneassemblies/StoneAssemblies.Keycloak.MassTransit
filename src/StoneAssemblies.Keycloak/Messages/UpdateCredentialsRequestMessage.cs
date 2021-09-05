// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateCredentialsRequestMessage.cs" company="Stone Assemblies">
//   Copyright © 2021 - 2021 Stone Assemblies. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace StoneAssemblies.Keycloak.Messages
{
    /// <summary>
    /// The update credentials request message.
    /// </summary>
    public class UpdateCredentialsRequestMessage : KeycloakMessage
    {
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }
    }
}