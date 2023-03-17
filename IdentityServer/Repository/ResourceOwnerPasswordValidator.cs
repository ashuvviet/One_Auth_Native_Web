using IdentityModel;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System;
using IdentityServer4.Validation;
using IdentityServer4.Models;

namespace IdentityServer.Repository
{
    /// <summary>
    /// It implements resource owner grant workflow
    /// </summary>
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly ILogger<ResourceOwnerPasswordValidator> _logger;

        /// <summary>
        /// Initializes an instance of the <see cref="ResourceOwnerPasswordValidator"/> class
        /// </summary>
        /// <param name="keycloakIdentityProviderServiceClient"></param>
        /// <param name="logger"></param>
        public ResourceOwnerPasswordValidator(
            ILogger<ResourceOwnerPasswordValidator> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            logger.LogInformation(
                $"Initializing {nameof(ResourceOwnerPasswordValidator)}...");
        }

        /// <summary>
        /// It validates username and password
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            _logger.LogInformation($"******** ResourceOwnerPasswordValidator ValidateAsync ***************");
            try
            {
                if (string.IsNullOrEmpty(context.UserName) || string.IsNullOrEmpty(context.Password))
                {
                    _logger.LogError("Null or empty username or password");
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest);
                }
                else
                {
                    //var tokenResponse = await
                    //    _keycloakIdentityProviderServiceClient.GetTokenUsingResourceOwnerPassword(
                    //        context.UserName, context.Password);

                    //_logger.LogInformation($"Password flow response from keycloak: {JsonConvert.SerializeObject(tokenResponse)}");

                    //context.Result = !string.IsNullOrEmpty(tokenResponse.AccessToken) &&
                    //                 !string.IsNullOrEmpty(tokenResponse.RefreshToken)
                    //    ? new GrantValidationResult(context.UserName,
                    //        OidcConstants.AuthenticationMethods.Password)
                    //    : new GrantValidationResult(TokenRequestErrors.InvalidGrant);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception occurred at {nameof(ResourceOwnerPasswordValidator)}");
            }
        }
    }
}
