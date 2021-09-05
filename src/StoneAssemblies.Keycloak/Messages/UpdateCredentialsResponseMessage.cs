// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateCredentialsResponseMessage.cs" company="Stone Assemblies">
//   Copyright © 2021 - 2021 Stone Assemblies. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace StoneAssemblies.Keycloak.Messages
{
    /// <summary>
    /// The update credentials response message.
    /// </summary>
    public class UpdateCredentialsResponseMessage : KeycloakMessage
    {
        /// <summary>
        /// Gets or sets a value indicating whether succeeded.
        /// </summary>
        public bool Succeeded { get; set; }
    }
}