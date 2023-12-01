using System.Security.Claims;
using Common.Logging;
using Microsoft.IdentityModel.Tokens;
using Ordering.Application;
using Ordering.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(SeriLogger.Configure);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication("Bearer")
    .AddCookie()
    .AddJwtBearer(options =>
    {
        options.Authority = "https://localhost:6001";
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

app.UseAuthorization();

app.MapControllers();

app.Run();