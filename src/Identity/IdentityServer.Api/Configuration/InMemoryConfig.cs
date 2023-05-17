using Duende.IdentityServer.Models;
using IdentityModel;

namespace IdentityServer.Api.Configuration;

// This could be moved into a DB, but for keeping things simpler the config is kept in code
public static class InMemoryConfig
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource("role", new List<string> { JwtClaimTypes.Role }),
            new IdentityResource("name", new List<string> { JwtClaimTypes.Name })
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new ApiScope("roles", new[] { JwtClaimTypes.Role })
        };

    public static IEnumerable<ApiResource> ApiResources =>
        new List<ApiResource>
        {
            new ApiResource("roles", new List<string> { JwtClaimTypes.Role })
        };

    public static IEnumerable<Client> Clients(IdentityConfiguration identityConfiguration)
        => identityConfiguration.ClientConfigurations.Select(clientConfiguration =>
            new Client
            {
                ClientId = clientConfiguration.ClientId,
                ClientSecrets = clientConfiguration.Secrets.Select(secret => new Secret(secret.Sha256())).ToList(),
                AllowedGrantTypes = clientConfiguration.AllowedGrantTypes.ToList(),
                RedirectUris = clientConfiguration.RedirectUris.ToList(),
                PostLogoutRedirectUris = clientConfiguration.PostLogoutRedirectUris.ToList(),
                AllowOfflineAccess = clientConfiguration.AllowOfflineAccess,
                AllowedScopes = clientConfiguration.AllowedScopes.ToList(),
                AlwaysIncludeUserClaimsInIdToken = clientConfiguration.AlwaysIncludeUserClaimsInIdToken,
                AlwaysSendClientClaims = clientConfiguration.AlwaysSendClientClaims,
                AccessTokenLifetime = clientConfiguration.AccessTokenLifetime
            });

}