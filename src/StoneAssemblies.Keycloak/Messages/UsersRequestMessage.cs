// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UsersRequestMessage.cs" company="Stone Assemblies">
//   Copyright © 2021 - 2021 Stone Assemblies. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace StoneAssemblies.Keycloak.Messages
{
    /// <summary>
    /// The users request message.
    /// </summary>
    public class UsersRequestMessage : KeycloakMessage
    {
        /// <summary>
        /// Gets or sets the take.
        /// </summary>
        public int Take { get; set; }

        /// <summary>
        /// Gets or sets the offset.
        /// </summary>
        public int Offset { get; set; }
    }
}