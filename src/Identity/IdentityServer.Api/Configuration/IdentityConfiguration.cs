namespace IdentityServer.Api.Configuration
{
    public class IdentityConfiguration
    {
        public IEnumerable<ClientConfiguration> ClientConfigurations { get; set; }
    }

    public class ClientConfiguration
    {
        public string ClientId { get; set; }
        public IEnumerable<string> Secrets { get; set; }
        public IEnumerable<string> AllowedGrantTypes { get; set; }
        public IEnumerable<string> RedirectUris { get; set; }
        public IEnumerable<string> PostLogoutRedirectUris { get; set; }
        public bool AllowOfflineAccess { get; set; }
        public IEnumerable<string> AllowedScopes { get; set; }
        public bool AlwaysIncludeUserClaimsInIdToken { get; set; }
        public bool AlwaysSendClientClaims { get; set; }
        public int AccessTokenLifetime { get; set; }
    }
}
