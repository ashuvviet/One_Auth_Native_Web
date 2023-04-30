﻿using IdentityModel;
using IdentityServerAspNetIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Novell.Directory.Ldap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;

namespace IdentityServerAspNetIdentity.Ldap
{
    /// <summary>
    /// This is an implementation of the service that is used to contact Ldap.
    /// </summary>
    public class LdapService<TUser> : ILdapService<TUser>
        where TUser : ApplicationUser, new()
    {
        private readonly ILogger<LdapService<TUser>> _logger;
        private readonly LdapConfig[] _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="LdapService{TUser}"/> class.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="logger">The logger.</param>
        public LdapService(IOptions<LdapConfig> config, ILogger<LdapService<TUser>> logger)
        {
            _logger = logger;
            _config = new [] { config.Value };
        }

        /// <summary>
        /// Logins using the specified credentials.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>
        /// Returns the logged in user.
        /// </returns>
        /// <exception cref="LoginFailedException">Login failed.</exception>
        public TUser Login(string username, string password)
        {
            return Login(username, password, null);
        }

        /// <summary>
        /// Logins using the specified credentials.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="domain">The domain friendly name.</param>
        /// <returns>
        /// Returns the logged in user.
        /// </returns>
        /// <exception cref="LoginFailedException">Login failed.</exception>
        public TUser Login(string username, string password, string domain)
        {
            var searchResult = SearchUser(username, domain);
            if (searchResult.Results != null && searchResult.Results.HasMore())
            {
                try
                {
                    var user = searchResult.Results.Next();
                    if (user != null)
                    {
                        searchResult.LdapConnection.Bind(user.Dn, password);
                        if (searchResult.LdapConnection.Bound)
                        {
                            //could change to ldap or change to configurable option
                            var provider = !string.IsNullOrEmpty(domain) ? domain : "local";
                            var appUser = new TUser();
                            SetBaseDetails(appUser, user, provider, searchResult.config.ExtraAttributes); // Should we change to LDAP.
                            searchResult.LdapConnection.Disconnect();

                            return appUser;
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.LogTrace(e.Message);
                    _logger.LogTrace(e.StackTrace);
                    throw new Exception("Login failed.", e);
                }
            }

            searchResult.LdapConnection.Disconnect();

            return default(TUser);
        }

        /// <summary>
        /// Finds user by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="domain">The domain friendly name.</param>
        /// <returns>
        /// Returns the user when it exists.
        /// </returns>
        public TUser FindUser(string username)
        {
            return FindUser(username, null);
        }

        /// <summary>
        /// Finds user by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="domain">The domain friendly name.</param>
        /// <returns>
        /// Returns the user when it exists.
        /// </returns>
        public TUser FindUser(string username, string domain)
        {
            var searchResult = SearchUser(username, domain);
            if (searchResult.Results != null)
            {
                try
                {
                    var user = searchResult.Results.Next();
                    if (user != null)
                    {
                        //could change to ldap or change to configurable option
                        var provider = !string.IsNullOrEmpty(domain) ? domain : "local";
                        var appUser = new TUser();
                        //appUser.UserName = username;
                        //appUser.Id = user.GetAttribute("id");
                        SetBaseDetails(appUser, user, provider, searchResult.config.ExtraAttributes);

                        searchResult.LdapConnection.Disconnect();

                        return appUser;
                    }
                }
                catch (Exception e)
                {
                    _logger.LogTrace(default(EventId), e, e.Message);
                    // Swallow the exception since we don't expect an error from this method.
                }
            }

            searchResult.LdapConnection.Disconnect();

            return default(TUser);
        }



        private (ILdapSearchResults Results, LdapConnection LdapConnection, LdapConfig config) SearchUser(string username, string domain)
        {
            //var allSearcheable = _config.Where(f => f.IsConcerned(username)).ToList();
            //if (!string.IsNullOrEmpty(domain))
            //{
            //    allSearcheable = allSearcheable.Where(e => e.FriendlyName.Equals(domain)).ToList();
            //}

            //if (allSearcheable == null || allSearcheable.Count() == 0)
            //{
            //    throw new Exception(
            //        "Login failed.",
            //        new Exception("No searchable LDAP"));
            //}

            var matchConfig = _config.First();
            using var ldapConnection = new LdapConnection { SecureSocketLayer = matchConfig.Ssl };
            // Could become async
            //foreach (var matchConfig in allSearcheable)
            //{
            //    using var ldapConnection = new LdapConnection { SecureSocketLayer = matchConfig.Ssl };
                ldapConnection.Connect(matchConfig.Url, matchConfig.FinalLdapConnectionPort);
                ldapConnection.Bind(matchConfig.BindDn, matchConfig.BindCredentials);

                var attributes = new TUser();//.LdapAttributes;
                var extrafieldList = new List<string>();

                if (matchConfig.ExtraAttributes != null)
                {
                    extrafieldList.AddRange(matchConfig.ExtraAttributes);
                }


                //attributes = attributes.Concat(extrafieldList).ToArray();

                var searchFilter = string.Format(matchConfig.SearchFilter, username);
                var result = ldapConnection.Search(
                    matchConfig.SearchBase,
                    LdapConnection.ScopeSub,
                    searchFilter,
                    extrafieldList.ToArray(),
                    false
                );

                if (result.HasMore()) // Count is async (not waiting). The hasMore() always works.
                {
                    return (Results: result as LdapSearchResults, LdapConnection: ldapConnection, matchConfig);
                }
            //}

            return (null, ldapConnection, matchConfig);
        }

        private void SetBaseDetails(TUser appusser, LdapEntry user, string provider, string[] extraAttributes)
        {
            FillClaims(appusser, user);

            FillExtrafields(appusser, user, extraAttributes);
        }

        public void FillClaims(TUser appusser, LdapEntry user)
        {
            // Example in LDAP we have display name as displayName (normal field)
            appusser.Claims = new List<Claim>
                {
                    GetClaimFromLdapAttributes(user, JwtClaimTypes.Name, OpenLdapAttributes.DisplayName),
                    GetClaimFromLdapAttributes(user, JwtClaimTypes.FamilyName, OpenLdapAttributes.LastName),
                    GetClaimFromLdapAttributes(user, JwtClaimTypes.GivenName, OpenLdapAttributes.FirstName),
                    GetClaimFromLdapAttributes(user, JwtClaimTypes.Email, OpenLdapAttributes.EMail),
                    GetClaimFromLdapAttributes(user, JwtClaimTypes.PhoneNumber, OpenLdapAttributes.TelephoneNumber)
                };

            // Add claims based on the user groups
            // add the groups as claims -- be careful if the number of groups is too large
            if (true)
            {
                try
                {
                    var userRoles = user.GetAttribute(OpenLdapAttributes.MemberOf.ToDescriptionString()).StringValues;
                    while (userRoles.MoveNext())
                    {
                        appusser.Claims.Add(new Claim(JwtClaimTypes.Role, userRoles.Current.ToString()));
                    }
                    //var roles = userRoles.Current (x => new Claim(JwtClaimTypes.Role, x.Value));
                    //id.AddClaims(roles);
                    //Claims = this.Claims.Concat(new List<Claim>()).ToList();
                }
                catch (Exception)
                {
                    // No roles exists it seems.
                }
            }
        }

        /// <summary>
        /// Fills the extra claims
        /// </summary>
        /// <param name="ldapEntry"></param>
        /// <param name="extrafields"></param>
        private void FillExtrafields(TUser appusser, LdapEntry user, IEnumerable<string> extrafields)
        {
            if (extrafields == null) return;

            var keyset = user.GetAttributeSet();
            foreach (var field in extrafields)
            {
                if (keyset.Keys.Contains(field))
                {
                    appusser.Claims.Add(new Claim(field, user.GetAttribute(field).StringValue));
                }
            }
        }

        public static string[] RequestedLdapAttributes()
        {
            throw new NotImplementedException();
        }

        internal Claim GetClaimFromLdapAttributes(LdapEntry user, string claim, OpenLdapAttributes ldapAttribute)
        {
            string value = string.Empty;

            try
            {
                value = user.GetAttribute(ldapAttribute.ToDescriptionString()).StringValue;
                return new Claim(claim, value);
            }
            catch (Exception)
            {
                // Should do something... But basically the attribute is not found
                // We swallow for now, since we might not care.
            }

            return new Claim(claim, value);
        }
    }

    public enum OpenLdapAttributes
    {
        [Description("displayName")]
        DisplayName,
        [Description("givenName")]
        FirstName,
        [Description("sn")] // Surname
        LastName,
        [Description("description")]
        Description,
        [Description("telephoneNumber")]
        TelephoneNumber,
        [Description("uid")] // Also used as user name
        Name,
        [Description("uid")]
        UserName,
        [Description("mail")]
        EMail,
        [Description("memberOf")] // Groups attribute that can appears multiple time
        MemberOf
    }

    public static class OpenLdapAttributesExtensions
    {
        /// <summary>
        /// Create from an <see cref="Enum"/> the description array.
        /// </summary>
        /// <typeparam name="T">An enum type</typeparam>
        /// <returns>An Array of the descriptions (no duplicate)</returns>
        /// <exception cref="ArgumentException">T must be an enumerated type</exception>
        public static Array ToDescriptionArray<T>()
            where T : IConvertible //,struct
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            List<string> result = new List<string>();
            foreach (var e in Enum.GetValues(typeof(T)))
            {
                var fi = e.GetType().GetField(e.ToString());
                var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                var description = attributes[0].Description;
                if (!result.Contains(description))
                {
                    result.Add(description);
                }
            }

            return result.ToArray();
        }

        public static string ToDescriptionString(this OpenLdapAttributes val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }
}
