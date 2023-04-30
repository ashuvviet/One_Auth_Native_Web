using Duende.IdentityServer;
using Duende.IdentityServer.EntityFramework.DbContexts;
using IdentityServerAspNetIdentity.Data;
using IdentityServerAspNetIdentity.Ldap;
using IdentityServerAspNetIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Reflection;

namespace IdentityServerAspNetIdentity;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        // LDAP
        builder.Services.Configure<LdapConfig>(builder.Configuration.GetSection("ldap"));
        builder.Services.AddSingleton<ILdapService<ApplicationUser>, LdapService<ApplicationUser>>();

        builder.Services.AddRazorPages();

        var migrationsAssembly = typeof(HostingExtensions).GetTypeInfo().Assembly.GetName().Name;
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
                            options.UseMySql(
                            connectionString, 
                            ServerVersion.AutoDetect(connectionString)));

        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            //.AddSignInManager<ApplicationSignInManager>()
            //.AddUserManager<LdapUserManager<ApplicationUser>>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        builder.Services
            .AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                // see https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/
                options.EmitStaticAudienceClaim = true;
            })
            //.AddOperationalStore(storeOptions =>
            //{
            //    storeOptions.ConfigureDbContext = b =>
            //           b.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), sql => sql.MigrationsAssembly(migrationsAssembly));

            //    // this enables automatic token cleanup. this is optional.
            //    storeOptions.EnableTokenCleanup = true;
            //    storeOptions.TokenCleanupInterval = 3600;
            //})
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryClients(Config.Clients)
            .AddExtensionGrantValidator<DelegationGrantValidator>()
            .AddAspNetIdentity<ApplicationUser>()
            .AddProfileService<CustomProfileService>();
        
        builder.Services.AddAuthentication()
             .AddOpenIdConnect("oidc", "Microsoft Azure AD", options =>
             {
                 options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                 options.SignOutScheme = IdentityServerConstants.SignoutScheme;
                 options.SaveTokens = true;

                 options.Authority = "https://login.microsoftonline.com/771c9c47-7f24-44dc-958e-34f8713a8394";
                 options.ClientId = "7795c084-82fe-4257-846c-64474d87f0a4";
                 options.ClientSecret = "66P8Q~4NfWMLO.joVL6Rnid7KsynonkFLw9avawb";
                 options.ResponseType = "code";

                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     NameClaimType = "name",
                     RoleClaimType = "role"
                 };
             })
            .AddGoogle(options =>
            {
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
            });

        return builder.Build();
    }
    
    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.Migrate();
        //using var grantdbContext = scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();
        //grantdbContext.Database.Migrate();

        app.UseSerilogRequestLogging();
    
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseStaticFiles();
        app.UseRouting();
        app.UseIdentityServer();
        app.UseAuthorization();
        
        app.MapRazorPages()
            .RequireAuthorization();

        return app;
    }
}