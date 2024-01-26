using System.Security.Claims;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;
using IdentityModel;

namespace IdentityServer;

public static class Config
{
    public static IEnumerable<ApiResource> ApiResources => new List<ApiResource>
    {
        new ApiResource("api", "Shop API")
        {
            UserClaims = { JwtClaimTypes.Role },
            Scopes = { "api" }
        },
    };
    
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email(),
            new IdentityResource("roles", "My Roles", new List<string>{ "role" })
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("api"),
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            // m2m client credentials flow client
            new Client
            {
                ClientId = "m2m.client",
                ClientName = "Client Credentials Client",

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                AllowedScopes = { "api" }
            },

            // interactive client using code flow + pkce
            new Client
            {
                ClientId = "interactive",
                ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = { "https://localhost:5011/signin-oidc" },
                FrontChannelLogoutUri = "https://localhost:5011/signout-oidc",
                PostLogoutRedirectUris = { "https://localhost:5011/signout-callback-oidc" },

                AllowOfflineAccess = true,
                AllowedScopes = { "openid", "profile", "email", "roles", "api" }
            },
            new Client
            {
                ClientId = "interactive.dev",
                ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = { "https://localhost:8011/signin-oidc" },
                FrontChannelLogoutUri = "https://localhost:8011/signout-oidc",
                PostLogoutRedirectUris = { "https://localhost:8011/signout-callback-oidc" },

                AllowOfflineAccess = true,
                AllowedScopes = { "openid", "profile", "email", "roles", "api" }
            }
        };
    
    public static List<TestUser> TestUsers =>
        new()
        {
            new TestUser
            {
                SubjectId = "5BE86359-073C-434B-AD2D-A3932222DABE",
                Username = "soekyawthu",
                Password = "12345",
                Claims =
                {
                    new Claim(JwtClaimTypes.Email, "soekyawthu.dev@gmail.com"),
                    new Claim(JwtClaimTypes.BirthDate, "01-08-2000"),
                    new Claim(JwtClaimTypes.Role, "admin"),
                    new Claim(JwtClaimTypes.Role, "member")
                }
            },
            new TestUser
            {
                SubjectId = "5BE86359-073C-434B-AD2D-A3932222DABF",
                Username = "alice",
                Password = "12345",
                Claims =
                {
                    new Claim(JwtClaimTypes.Email, "alice.dev@gmail.com"),
                    new Claim(JwtClaimTypes.BirthDate, "25-05-2000"),
                    new Claim(JwtClaimTypes.Role, "member")
                }
            }
        };
}