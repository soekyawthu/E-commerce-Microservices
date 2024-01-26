namespace IdentityServer;

public class Configuration
{
    public string? ClientId { get; set; }
    
    public string? ClientSecret { get; set; }

    public List<string> Scopes { get; set; } = new();
    public List<ApiClient> Clients { get; set; } = new();
}

public class ApiClient
{
    public string? RedirectUris { get; set; }
    public string? FrontChannelLogoutUri { get; set; }
    public string? PostLogoutRedirectUris { get; set; }
}