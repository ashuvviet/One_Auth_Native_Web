using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using IdentityServerAspNetIdentity.Models;
using System.Security.Claims;

namespace IdentityServerAspNetIdentity.Ldap
{
    /// <summary>
    /// Provides a custom user store that overrides password related methods to valid the user's password against LDAP.
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    public class LdapUserManager<TUser> : UserManager<TUser>
        where TUser : ApplicationUser, new()
    {
        private readonly ILdapService<ApplicationUser> _ldapService;

        /// <summary>
        /// Initializes an instance.
        /// </summary>
        /// <param name="store"></param>
        /// <param name="optionsAccessor"></param>
        /// <param name="passwordHasher"></param>
        /// <param name="userValidators"></param>
        /// <param name="passwordValidators"></param>
        /// <param name="keyNormalizer"></param>
        /// <param name="errors"></param>
        /// <param name="services"></param>
        /// <param name="logger"></param>
        /// <param name="ldapOptions"></param>
        public LdapUserManager(
            IUserStore<TUser> store, 
            IOptions<IdentityOptions> optionsAccessor, 
            IPasswordHasher<TUser> passwordHasher, 
            IEnumerable<IUserValidator<TUser>> userValidators, 
            IEnumerable<IPasswordValidator<TUser>> passwordValidators, 
            ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, 
            IServiceProvider services, ILogger<UserManager<TUser>> logger,
            ILdapService<ApplicationUser> ldapService
        ) : base(
            store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger
        )
        {
            _ldapService = ldapService;
        }

        /// <summary>
        /// Checks the given password agains the configured LDAP server.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public override async Task<bool> CheckPasswordAsync(TUser user, string password)
        {
            var result = await base.CheckPasswordAsync(user, password);
            if (!result)
            {

                var ldapResult = _ldapService.Login(user.UserName, password);
            }

            return result;
        }

        public override async Task<TUser> FindByNameAsync(string userName)
        {
            var user = await base.FindByNameAsync(userName);
            if(user == null)
            {
                user = (TUser)_ldapService.FindUser(userName);
            }

            return user;
        }

        public override Task<string> GetUserIdAsync(TUser user)
        {
            return Task.FromResult("X");
        }

        public override Task<string> GetUserNameAsync(TUser user)
        {
            return Task.FromResult("XY");
        }

        public override Task<string> GetEmailAsync(TUser user)
        {
            return Task.FromResult("XY@gmail.com");
        }

        public override Task<IList<Claim>> GetClaimsAsync(TUser user)
        {
            var list = new List<Claim> { new Claim("Name", "X") };
            return Task.FromResult<IList<Claim>>(list);
        }

    }

}
