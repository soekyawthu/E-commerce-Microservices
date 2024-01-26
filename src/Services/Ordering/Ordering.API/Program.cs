using System.Security.Claims;
using Common.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Ordering.Application;
using Ordering.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
//IdentityModelEventSource.ShowPII = true;

builder.Host.UseSerilog(SeriLogger.Configure);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/*builder.Services.AddCors(options => 
    options.AddDefaultPolicy( policy => policy
        .WithOrigins("https://localhost:3000")
        .AllowCredentials()
        .AllowAnyHeader()
        .AllowAnyMethod()));*/

/*builder.Services.AddAuthentication("token")
    .AddJwtBearer("token", options =>
    {
        Console.WriteLine($"Authority => {builder.Configuration["Identity:Authority"]}");
        options.Authority = builder.Configuration["Identity:Authority"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            NameClaimType = ClaimTypes.Name,
            RoleClaimType = ClaimTypes.Role,
        };
    });*/

builder.Services.AddAuthentication("token")
    .AddJwtBearer("token", options =>
    {
        options.Authority = "https://demo.duendesoftware.com";
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateAudience = false,
            NameClaimType = ClaimTypes.Name,
            RoleClaimType = ClaimTypes.Role,
        };
    });

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();