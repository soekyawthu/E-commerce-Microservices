using Common.Logging;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
using Shopping.Aggregator;
using Shopping.Aggregator.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(SeriLogger.Configure);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<LoggingDelegatingHandler>();

builder.Services.AddHttpClient<ICatalogService, CatalogService>(client =>
        client.BaseAddress = new Uri(builder.Configuration["ApiSettings:CatalogUrl"]!))
    .AddHttpMessageHandler<LoggingDelegatingHandler>()
    .AddPolicyHandler(HttpPolicy.GetRetryPolicy())
    .AddPolicyHandler(HttpPolicy.GetCircuitBreakerPolicy());

builder.Services.AddHttpClient<IBasketService, BasketService>(client =>
        client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BasketUrl"]!))
    .AddHttpMessageHandler<LoggingDelegatingHandler>()
    .AddPolicyHandler(HttpPolicy.GetRetryPolicy())
    .AddPolicyHandler(HttpPolicy.GetCircuitBreakerPolicy());

builder.Services.AddHttpClient<IOrderService, OrderService>(client =>
        client.BaseAddress = new Uri(builder.Configuration["ApiSettings:OrderingUrl"]!))
    .AddHttpMessageHandler<LoggingDelegatingHandler>()
    .AddPolicyHandler(HttpPolicy.GetRetryPolicy())
    .AddPolicyHandler(HttpPolicy.GetCircuitBreakerPolicy());

builder.Services.AddHealthChecks()
    .AddUrlGroup(new Uri(builder.Configuration["ApiSettings:CatalogUrl"]!), "Catalog Service")
    .AddUrlGroup(new Uri(builder.Configuration["ApiSettings:BasketUrl"]!), "Basket Service")
    .AddUrlGroup(new Uri(builder.Configuration["ApiSettings:OrderingUrl"]!), "Order Service");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapControllers();

app.Run();