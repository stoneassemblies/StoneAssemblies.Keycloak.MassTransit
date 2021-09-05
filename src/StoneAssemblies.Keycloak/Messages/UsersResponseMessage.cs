// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UsersResponseMessage.cs" company="Stone Assemblies">
// Copyright © 2021 - 2021 Stone Assemblies. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace StoneAssemblies.Keycloak.Messages
{
    using System.Collections.Generic;

    using StoneAssemblies.Keycloak.Models;

    /// <summary>
    /// The users response message.
    /// </summary>
    public class UsersResponseMessage : KeycloakMessage
    {
        /// <summary>
        /// Gets or sets the users.
        /// </summary>
        public List<User> Users { get; set; }
    }
}