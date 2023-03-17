using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.Repository
{
    /// <summary>
    /// Service for exposing custom User property in tokens
    /// </summary>
    internal class ProfileService : IProfileService
    {

        private readonly ILogger<ProfileService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileService"></see>
        /// </summary>
        /// <param name="authZClient">The authentication z client.</param>
        /// <param name="iIdentityProviderServiceClient">The i identity provider service client.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="tenantReadViewServiceClient"></param>
        public ProfileService(
            ILogger<ProfileService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            logger.LogInformation($"Initializing {nameof(ProfileService)}...");
        }

        /// <summary>
        /// API that is expected to load claims for a user
        /// </summary>
        /// <param name="context">Instance of ProfileDataRequestContext</param>
        /// <returns></returns>
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            //var userClaims = GetUserClaims(context);

            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Name,"Username"), // Name claim
                new Claim(JwtClaimTypes.Email, "UserEmail"),
                //new("FirstName", userClaims.FirstName),
                //new("LastName", userClaims.LastName),
                //new("UserId", userClaims.UserId)
            };

            //var getRolesResponse = GetRoles(context, userClaims.UserId);

            //_logger.LogInformation("GetRolesResponse: " + JsonConvert.SerializeObject(getRolesResponse));

            //if (getRolesResponse == null || getRolesResponse.Roles.Count == 0)
            //{
            //    throw new Exception("No roles exist for this user or some error occurred");
            //}
            //else
            //{
            //    // Add role claim
            //    claims.AddRange(getRolesResponse.Roles.Select(role =>
            //        new Claim(JwtClaimTypes.Role, role)));
            //}

            //if (getRolesResponse.SubTenants != null)
            //{
            //    claims.AddRange(getRolesResponse.SubTenants.Select(tenant =>
            //        new Claim("SubTenants", tenant)));
            //}

            //if (context.Subject.GetAuthenticationMethod() == OidcConstants.GrantTypes.TokenExchange)
            //{
            //    claims.Add(new("TenantId", context.Subject.Claims.FirstOrDefault(c => c.Type == "TenantId")?.Value));
            //}
            //else
            //{
            //    claims.Add(new("TenantId", getRolesResponse.TenantId));
            //}

            //var tenantReadViewResponse = _tenantReadViewServiceClient.GetTenantDetails(getRolesResponse.TenantId);
            //tenantReadViewResponse.Wait();
            //claims.Add(new Claim("TenantName", tenantReadViewResponse?.Result?.Name));
            //claims.Add(new Claim("TopLevelTenantId", tenantReadViewResponse?.Result?.AggregateId));
            context.IssuedClaims = claims;

            return Task.FromResult(0);
        }       


        /// <summary>
        /// API that is expected to indicate if a user is currently allowed to obtain tokens
        /// </summary>
        /// <param name="context">Instance of IsActiveContext</param>
        /// <returns></returns>
        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject?.GetSubjectId();
            if (string.IsNullOrEmpty(sub))
            {
                throw new Exception("No subject Id claim present");
            }
            else
            {
                await IsActiveAsync(context, sub);
            }
        }
        private async Task IsActiveAsync(IsActiveContext context, string subjectId)
        {
            _logger.LogInformation($"IsActiveAsync:  subject id {subjectId}");
            //var user = await _identityProviderServiceClient.CheckIfUsernameExists(subjectId);

            _logger.LogInformation($"************** Conext Caller from IsActiveAsync {context.Caller}");

            //if (context.Caller.Equals("AccessTokenValidation", StringComparison.OrdinalIgnoreCase))
            //{
            //    if (user != null)
            //        context.IsActive = !VerifyRenewToken(subjectId, context.Client.ClientId);
            //    else
            //        context.IsActive = false;
            //}
            //else
            //    context.IsActive = user != null;

            context.IsActive = true;
        }
    }
}
