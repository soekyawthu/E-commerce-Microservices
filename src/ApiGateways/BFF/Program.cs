using Duende.Bff.Yarp;
using BFF;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile($"yarp.{builder.Environment.EnvironmentName}.json", true, true);

IdentityModelEventSource.ShowPII = true;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(x => 
    x.AddDefaultPolicy(policy => policy
            .WithOrigins("https://localhost:3000")
            .AllowCredentials()
            .AllowAnyHeader()
            .AllowAnyMethod()));

builder.Services.AddBff();
    //.AddRemoteApis();

builder.Services
    .AddReverseProxy()
    .AddBffExtensions()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

Configuration config = new();
builder.Configuration.Bind("BFF", config);

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = "cookie";
        options.DefaultChallengeScheme = "oidc";
        options.DefaultSignOutScheme = "oidc";
    })
    .AddCookie("cookie", options =>
    {
        options.Cookie.Name = "__Host-bff";
        options.Cookie.SameSite = SameSiteMode.Strict;
    })
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = config.Authority;
        options.ClientId = config.ClientId;
        options.ClientSecret = config.ClientSecret;
        options.RequireHttpsMetadata = false;
        options.ResponseType = "code";
        options.ResponseMode = "query";

        options.GetClaimsFromUserInfoEndpoint = true;
        options.MapInboundClaims = false;
        options.SaveTokens = true;
    
        options.Scope.Clear();
        foreach (var scope in config.Scopes)
        {
            options.Scope.Add(scope);
        }

        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = "name",
            RoleClaimType = "role"
        };
    });


var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseBff();
app.UseAuthorization();

app.MapBffManagementEndpoints();
app.MapBffReverseProxy()
    .SkipAntiforgery();

app.MapGet("token", async context =>
{
    var token = await context.GetUserAccessTokenAsync();
    await context.Response.WriteAsJsonAsync(token);
});
/*if (config.Apis.Any())
{
    foreach (var api in config.Apis)
    {
        app.MapRemoteBffApiEndpoint(api.LocalPath, api.RemoteUrl!)
            .SkipAntiforgery();
        //.RequireAccessToken(api.RequiredToken);
    }
}*/

app.Run();