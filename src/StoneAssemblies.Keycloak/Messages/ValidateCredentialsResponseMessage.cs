// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidateCredentialsResponseMessage.cs" company="Stone Assemblies">
//   Copyright © 2021 - 2021 Stone Assemblies. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace StoneAssemblies.Keycloak.Messages
{
    /// <summary>
    /// The validate credentials response message.
    /// </summary>
    public class ValidateCredentialsResponseMessage : KeycloakMessage
    {
        /// <summary>
        /// Gets or sets a value indicating whether succeeded.
        /// </summary>
        public bool Succeeded { get; set; }
    }
}