using System.Security.Claims;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace UserManagement;

public class Config
{
    public static IEnumerable<ApiResource> ApiResources => new List<ApiResource>
    {
        new ApiResource("shopApi", "Shop API")
        {
            UserClaims = { JwtClaimTypes.Role },
            Scopes = { "shopApi" }
        },
    };
    
    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new("shopApi", "Shop API"),
        };

    public static IEnumerable<Client> Clients => 
        new List<Client>
        {
            new() 
            {
                ClientId = "shop_client",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("secret".Sha256()) }
            },
            new()
            {
                ClientId = "shop_mvc_client",
                AllowedGrantTypes = GrantTypes.Hybrid,
                RequirePkce = false,
                ClientSecrets = { new Secret("secret".Sha256()) },
                RedirectUris =
                {
                    "https://localhost:6002/signin-oidc"
                },
                PostLogoutRedirectUris =
                {
                    "https://localhost:6002/signout-callback-oidc"
                },
                AllowedScopes =
                {
                    "shopApi", 
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    "roles"
                }
            }
        };

    public static List<IdentityResource> IdentityResources =>
        new()
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email(),
            new IdentityResource("roles", "My Roles", new List<string>{ "role" })
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