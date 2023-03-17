// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer.LdapExtension;
using IdentityServer.LdapExtension.Extensions;
using IdentityServer.LdapExtension.UserModel;
using IdentityServer.LdapExtension.UserStore;
using IdentityServer.Repository;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServerHost.Quickstart.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace IdentityServer
{
    public class MyRedirectValidator : IdentityServer4.Validation.IRedirectUriValidator
    {
        public Task<bool> IsPostLogoutRedirectUriValidAsync(string requestedUri, Client client)
        {
            var myUri = new Uri(requestedUri);
            string domainUri = requestedUri.Substring(0, requestedUri.IndexOf(myUri.Authority) + myUri.Authority.Length);
            var pattern1 = string.Concat(domainUri, "/", "signout-oidc");
            var pattern2 = string.Concat(domainUri, "/", "#", "/", "signout-oidc");
            foreach (string uri in client.PostLogoutRedirectUris)
            {
                if (Regex.Match(uri, pattern1, RegexOptions.IgnoreCase).Success ||
                    Regex.Match(uri, pattern2, RegexOptions.IgnoreCase).Success)
                {
                    return Task.FromResult(true);
                }
            }

            return Task.FromResult(true);
        }

        /// <summary>
        /// IsRedirectUriValidAsync
        /// </summary>
        /// <param name="requestedUri"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public Task<bool> IsRedirectUriValidAsync(string requestedUri, Client client)
        {
            var myUri = new Uri(requestedUri);
            string domainUri = requestedUri.Substring(0, requestedUri.IndexOf(myUri.Authority) + myUri.Authority.Length);
            var pattern1 = string.Concat(domainUri, "/", "signin-oidc");
            var pattern2 = string.Concat(domainUri, "/", "#", "/", "signin-oidc");
            var pattern3 = string.Concat(domainUri, "/", "Home", "/", "signin-oidc");

            foreach (string uri in client.RedirectUris)
            {
                if (Regex.Match(uri, pattern1, RegexOptions.IgnoreCase).Success ||
                    Regex.Match(uri, pattern2, RegexOptions.IgnoreCase).Success)
                {
                    return Task.FromResult(true);
                }
            }

            return Task.FromResult(true);
        }
    }

    public class HostProfileService : LdapUserProfileService
    {
        public HostProfileService(ILdapUserStore users, ILogger<LdapUserProfileService> logger) : base(users, logger)
        {
        }

        public override async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            await base.GetProfileDataAsync(context);

            var transaction = context.RequestedResources.ParsedScopes.FirstOrDefault(x => x.ParsedName == "transaction");
            if (transaction?.ParsedParameter != null)
            {
                context.IssuedClaims.Add(new Claim("transaction_id", transaction.ParsedParameter));
            }
        }
    }

    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;

            IdentityModelEventSource.ShowPII = true;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddTransient<IdentityServer4.Validation.IRedirectUriValidator, MyRedirectValidator>();
            var c = _config.GetSection("IdentityServerLdap");
            var p = c.GetSection("Connections").GetChildren().First().GetValue<string>("FriendlyName");
            var builder = services.AddIdentityServer(opt =>
            {
                //opt.Authentication.CookieLifetime = new System.TimeSpan(0, 0, 90);
                //opt.Authentication.CookieSlidingExpiration = true;
            })
                .AddInMemoryIdentityResources(Config.IdentityResources)
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddInMemoryClients(Config.Clients)
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
                //.AddExtensionGrantValidator<TokenExchangeGrantValidator>()
                //.AddProfileService<ProfileService>()
                //.AddTestUsers(TestUsers.Users)
                .AddLdapUsers<OpenLdapAppUser>(_config.GetSection("IdentityServerLdap"), UserStore.Redis)
                //.AddProfileService<HostProfileService>();
                //.AddRedirectUriValidator<MyRedirectValidator>()
                ;

            builder.AddDeveloperSigningCredential();

            services.Configure<CookiePolicyOptions>(cookiePolicyOptions =>
            {
                cookiePolicyOptions.MinimumSameSitePolicy = SameSiteMode.None;
                cookiePolicyOptions.Secure = CookieSecurePolicy.Always;
            });

            // services.AddDataProtection();

            //services.AddOidcStateDataFormatterCache("aad", "demoidsrv");
            services.AddAuthentication()
               .AddOpenIdConnect("IS4", "Microsoft Azure AD", options =>
               {
                   options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                   options.SignOutScheme = IdentityServerConstants.SignoutScheme;
                   options.SaveTokens = true;

                   options.Authority = "https://login.microsoftonline.com/771c9c47-7f24-44dc-958e-34f8713a8394";
                   options.ClientId = "d1cc3af6-a5a4-4f2b-aea9-2fe3bb21ec59";
                   options.ClientSecret = "rr28Q~.pYgxAVj4d~p6qS71WJ.EMTe_3lQ3JeaEy";
                   options.ResponseType = "code";

                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       NameClaimType = "name",
                       RoleClaimType = "role"
                   };
               });
            //.AddOpenIdConnect("oidc", "Sciex OS", connectOptions =>
            //{
            //    connectOptions.RequireHttpsMetadata = false;
            //    connectOptions.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
            //    connectOptions.ResponseType = OpenIdConnectResponseType.Code;
            //    connectOptions.Authority = "http://localhost:8080/auth/realms/platform";
            //    connectOptions.ClientId = "mvc";
            //    connectOptions.ClientSecret = "3ae80d81-dc64-46dd-85fc-1526903f64e1";
            //    connectOptions.GetClaimsFromUserInfoEndpoint = true;
            //    connectOptions.SaveTokens = true;
            //    //connectOptions.SignedOutCallbackPath = "/signout-callback-oidcXXX";
            //    //connectOptions.SignedOutCallbackPath = new PathString("/signout-callback-oidc");
            //    //connectOptions.RemoteSignOutPath = new PathString("/signout-oidc");
            //    //connectOptions.SignedOutRedirectUri = "https://localhost:9999/index.html";
            //    connectOptions.RemoteAuthenticationTimeout = TimeSpan.FromSeconds(60);
            //    connectOptions.CorrelationCookie.SameSite = SameSiteMode.None;
            //    connectOptions.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
            //    connectOptions.NonceCookie.SameSite = SameSiteMode.None;
            //    connectOptions.NonceCookie.SecurePolicy = CookieSecurePolicy.Always;
            //    connectOptions.RemoteAuthenticationTimeout = TimeSpan.FromSeconds(300);

            //    connectOptions.Events.OnRemoteFailure += context =>
            //    {
            //        if (context.Failure.Message.Contains("Correlation failed"))
            //        {
            //            var returnurl = context.Properties.Items["returnUrl"];
            //            //var xx = @"/connect/authorize/callback?client_id=js&redirect_uri=https%3A%2F%2Flocalhost%3A5013%2Fcallback.html&response_type=code&scope=openid%20profile%20api1&state=0084b563d19f44029a557e7e2614de97&code_challenge=JOg2otED7udiZm6DKpVuAj2-HnrKJm39moTqXoVHW0k&code_challenge_method=S256&response_mode=query";
            //            string param1 = HttpUtility.ParseQueryString(returnurl).Get("redirect_uri");
            //            Uri myUri = new Uri(param1);
            //            string domainUri = param1.Substring(0, param1.IndexOf(myUri.Authority) + myUri.Authority.Length);

            //            context.Response.Redirect(domainUri);
            //        }

            //        context.HandleResponse();

            //        return Task.CompletedTask;
            //    };

            //    connectOptions.Events.OnRedirectToIdentityProvider += context =>
            //    {

            //        Console.WriteLine(context.ProtocolMessage);
            //        return Task.CompletedTask;
            //    };

            //    connectOptions.Events.OnRedirectToIdentityProviderForSignOut += context =>
            //    {
            //        //string appBaseUrl = context.Request.Scheme + "://" + context.Request.Host + context.Request.PathBase;
            //        context.ProtocolMessage.PostLogoutRedirectUri = @"https://localhost:5001/signout-callback-oidc";
            //        Console.WriteLine(context.ProtocolMessage);
            //        return Task.CompletedTask;
            //    };
            //});
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();

            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
