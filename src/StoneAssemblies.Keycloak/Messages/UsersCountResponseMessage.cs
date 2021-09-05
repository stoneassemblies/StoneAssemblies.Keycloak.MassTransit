// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UsersCountResponseMessage.cs" company="Stone Assemblies">
//   Copyright © 2021 - 2021 Stone Assemblies. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace StoneAssemblies.Keycloak.Messages
{
    /// <summary>
    /// The users count response message.
    /// </summary>
    public class UsersCountResponseMessage : KeycloakMessage
    {
        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        public int Count { get; set; }
    }
}