using Duende.IdentityServer.Models;

namespace IdentityService;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        [
            new ApiScope("auctionApp", "Auction App full access")
        ];

    public static IEnumerable<Client> Clients =>
        [
            new Client
            {
                ClientId = "nextApp",
                ClientName = "Next.js App",
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                RequirePkce = false,
                RedirectUris = { "http://localhost:3000/api/auth/callback/id-server" },
                AllowOfflineAccess = true,
                AllowedScopes = {
                    "openid",
                    "profile",
                    "auctionApp"
                },
                AccessTokenLifetime = 3600*24*30
            }
        ];
}
