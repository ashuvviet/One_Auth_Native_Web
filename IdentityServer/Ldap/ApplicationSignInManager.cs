using IdentityServerAspNetIdentity.Data;
using IdentityServerAspNetIdentity.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace IdentityServerAspNetIdentity.Ldap
{
    public class ApplicationSignInManager : SignInManager<ApplicationUser>
    {
        private readonly ILdapService<ApplicationUser> _ldapService;

        public ApplicationSignInManager(
    UserManager<ApplicationUser> userManager,
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<ApplicationUser>> logger,
            IAuthenticationSchemeProvider schemeProvider,
            IUserConfirmation<ApplicationUser> confirmation,
            ILdapService<ApplicationUser> ldapService
            ) : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemeProvider, confirmation)
        {

            _ldapService = ldapService ?? throw new ArgumentNullException(nameof(ldapService));
        }

        public override async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure)
        { 
            var result = await base.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure);
            if (!result.Succeeded)
            {
                var ldapResult = _ldapService.Login(userName, password);
                if(ldapResult != null)
                {
                    result = new SignResult(true);
                }
            }

            return result;
        }
    }

    public class SignResult : SignInResult { 
        public SignResult(bool success) {
            Succeeded = success;
        }
    }
}
