// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FindUserByIdRequestMessage.cs" company="Stone Assemblies">
//   Copyright © 2021 - 2021 Stone Assemblies. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace StoneAssemblies.Keycloak.Messages
{
    /// <summary>
    ///     The find user by id request message.
    /// </summary>
    public class FindUserByIdRequestMessage : KeycloakMessage
    {
        /// <summary>
        ///     Gets or sets the user id.
        /// </summary>
        public string UserId { get; set; }
    }
}