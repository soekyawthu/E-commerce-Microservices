using BFF.Web;
using Duende.Bff.Yarp;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile($"yarp.{builder.Environment.EnvironmentName}.json", true, true);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(x => 
    x.AddDefaultPolicy(policy => 
        policy.WithOrigins("https://localhost:3000")
        .AllowCredentials()
        .AllowAnyHeader()
        .AllowAnyMethod()));

builder.Services.AddBff();

builder.Services.AddReverseProxy()
    .AddBffExtensions()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

Configuration config = new();
builder.Configuration.Bind("BFF", config);

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "cookie";
    options.DefaultChallengeScheme = "oidc";
    options.DefaultSignOutScheme = "oidc";
}).AddCookie("cookie", options =>
{
    options.Cookie.Name = "__Host-bff";
    options.Cookie.SameSite = SameSiteMode.Strict;
}).AddOpenIdConnect("oidc", options =>
{
    options.Authority = config.Authority;
    options.ClientId = config.ClientId;
    options.ClientSecret = config.ClientSecret;
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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseBff();
app.UseAuthorization();
app.MapBffManagementEndpoints();
app.MapBffReverseProxy();//SkipAntiforgery();

app.MapGet("", context =>
{
    context.Response.Redirect("https://localhost:3000");
    return Task.CompletedTask;
});

app.Run();

/*

using Duende.Bff.Yarp;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile($"yarp.{builder.Environment.EnvironmentName}.json", true, true);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(x => 
    x.AddDefaultPolicy(policy => 
        policy.WithOrigins("https://localhost:3000")
        .AllowCredentials()
        .AllowAnyHeader()
        .AllowAnyMethod()));

builder.Services.AddBff();
    //.AddRemoteApis();

builder.Services.AddReverseProxy()
    //.AddTransforms<AccessTokenTransformProvider>();
    .AddBffExtensions()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "cookie";
    options.DefaultChallengeScheme = "oidc";
    options.DefaultSignOutScheme = "oidc";
}).AddCookie("cookie", options =>
{
    options.Cookie.Name = "__Host-bff";
    options.Cookie.SameSite = SameSiteMode.Strict;
}).AddOpenIdConnect("oidc", options =>
{
    options.Authority = "https://demo.duendesoftware.com";
    options.ClientId = "interactive.confidential";
    options.ClientSecret = "secret";
    options.ResponseType = "code";
    options.ResponseMode = "query";

    options.GetClaimsFromUserInfoEndpoint = true;
    options.MapInboundClaims = false;
    options.SaveTokens = true;

    options.Scope.Clear();
    options.Scope.Add("api");
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("offline_access");

    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = "name",
        RoleClaimType = "role"
    };
});

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseBff();
app.UseAuthorization();

/*app.MapControllers()
    .AsBffApiEndpoint();* /

//app.MapFallbackToFile("/index.html");

app.MapBffManagementEndpoints();
app.MapBffReverseProxy();
//app.MapReverseProxy();
/*app.MapRemoteBffApiEndpoint("/wf", "https://localhost:7103/api/weatherforecast")
    .RequireAccessToken();* /

app.MapGet("", context =>
{
    context.Response.Redirect("https://localhost:3000");
    return Task.CompletedTask;
});

app.MapGet("api/account", async context =>
{
    var token = await context.GetUserAccessTokenAsync();
    //var items = (await HttpContext.AuthenticateAsync()).Properties?.Items;
    await context.Response.WriteAsJsonAsync(new { token });
});


app.Run();

 */