namespace IdentityServerAspNetIdentity
{
    using Duende.IdentityServer.Models;
    using Duende.IdentityServer.Validation;
    using IdentityModel;

    public class DelegationGrantValidator : IExtensionGrantValidator
    {
        private readonly ITokenValidator _validator;

        public DelegationGrantValidator(ITokenValidator validator)
        {
            _validator = validator;
        }

        public string GrantType => OidcConstants.GrantTypes.TokenExchange;

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest);

            var subjectToken = context.Request.Raw.Get(OidcConstants.TokenRequest.SubjectToken);
            var subjectTokenType = context.Request.Raw.Get(OidcConstants.TokenRequest.SubjectTokenType);

            // mandatory parameters
            if (string.IsNullOrWhiteSpace(subjectToken))
            {
                return;
            }

            if (!string.Equals(subjectTokenType, OidcConstants.TokenTypeIdentifiers.AccessToken))
            {
                return;
            }

            var validationResult = await _validator.ValidateAccessTokenAsync(subjectToken);
            if (validationResult.IsError)
            {
                return;
            }

            // get user's identity
            var sub = validationResult.Claims.First(c => c.Type == "sub").Value;
            context.Result = new GrantValidationResult(sub, GrantType);
        }
    }
}
