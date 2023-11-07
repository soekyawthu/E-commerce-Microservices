using Basket.API.GrpcServices;
using Basket.Application;
using Basket.Infrastructure;
using Common.Logging;
using Discount.Grpc;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("MassTransit", LogEventLevel.Debug)
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

//builder.Host.UseSerilog(SeriLogger.Configure);
builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
});

/*builder.Services.AddHealthChecks()
    .AddRedis(builder.Configuration["CacheSettings:ConnectionString"]!, "Redis");*/

builder.Services.AddScoped<DiscountService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

/*app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});*/

app.MapControllers();

app.Run();