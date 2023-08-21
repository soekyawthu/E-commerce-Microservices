using Common.Logging;
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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();