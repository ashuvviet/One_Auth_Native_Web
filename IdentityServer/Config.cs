using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace IdentityServerAspNetIdentity;

using IdentityModel;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource("color", new [] { "favorite_color" })
        };


    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new ApiScope("api1", "first api"),
            new ApiScope("QCAPI", "quant on cloud")
        };

    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            // machine to machine client
            new Client
            {
                ClientId = "client",
                ClientSecrets = { new Secret("secret".Sha256()) },

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                // scopes that client has access to
                AllowedScopes = { "api1" }
            },
                
            // interactive ASP.NET Core Web App
            new Client
            {
                ClientId = "mvc",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                    
                    // where to redirect to after logimvcn
                    RedirectUris = { "https://localhost:5012/signin-oidc", "https://localhost:8000/Home/signin-oidc", "com.sciex://call", "sample-windows-client://callback",  "sample-windows-client://call" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" },

                    AllowOfflineAccess= true,
                    AbsoluteRefreshTokenLifetime = 900,
                    SlidingRefreshTokenLifetime = 120,
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    AccessTokenLifetime = 60,
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                       "api1"
                    },
                    Claims =
                    {
                        new ClientClaim("sub", "123")
                    }
            },

            new Client
            {
                ClientId = "js",
                ClientSecrets = { new Secret("secret".Sha256()) },
                ClientName = "JavaScript Client",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                RequireClientSecret = false,

                    AccessTokenLifetime = 3600 * 24,
                AbsoluteRefreshTokenLifetime = 3600 * 24 * 30,
                SlidingRefreshTokenLifetime = 3600 * 24 * 30,
                RefreshTokenExpiration = TokenExpiration.Sliding,
                AllowOfflineAccess = true,
                // UserSsoLifetime = 60,
                //RequirePkce = true,
                RedirectUris =           { "https://localhost:5013/signin-oidc", "https://localhost:5013/callback.html", "https://localhost:5013/identityserver-silent.html" },
                PostLogoutRedirectUris = { "https://localhost:5013/index.html", "https://localhost:6002/index.html" },
                AllowedCorsOrigins =     { "https://localhost:5013", "https://localhost:6002" },

                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "api1"
                }
            },

            new Client
            {
                ClientId = "qc",
                ClientSecrets = { new Secret("secret".Sha256()) },
                ClientName = "qc Client",
                AllowedGrantTypes = { OidcConstants.GrantTypes.TokenExchange },
                RequireClientSecret = false,

                AccessTokenLifetime = 3600 * 24,
                AbsoluteRefreshTokenLifetime = 3600 * 24 * 30,
                SlidingRefreshTokenLifetime = 3600 * 24 * 30,
                RefreshTokenExpiration = TokenExpiration.Sliding,
                AllowOfflineAccess = true,

                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.OfflineAccess,
                    "QCAPI"
                }
            }
        };
}