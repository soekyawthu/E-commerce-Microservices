using System.Reflection;
using Basket.Application.Contracts.Infrastructure;
using Basket.Application.Contracts.Persistence;
using Basket.Domain.Entities;
using Basket.Infrastructure.Extensions;
using Basket.Infrastructure.Repositories;
using Basket.Infrastructure.Services;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Basket.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        var connectionString = config["DatabaseSettings:ConnectionString"];//config.GetConnectionString("Default");
        const string databaseName = "ShoppingCart";

        services.AddSingleton<IMongoClient>(_ => new MongoClient(connectionString));
        services.AddSingleton<IMongoDatabase>(provider => provider.GetRequiredService<IMongoClient>().GetDatabase(databaseName));
        services.AddMongoDbCollection<ShoppingCart>(x => x.ShoppingCartId);

        services.AddScoped<IBasketRepository, BasketRepository>();
        services.AddScoped<ICheckoutService, CheckoutService>();
        
        services.AddMassTransit(configurator =>
        {
            configurator.SetKebabCaseEndpointNameFormatter();
            configurator.AddMongoDbOutbox(o =>
            {
                o.DisableInboxCleanupService();
                o.ClientFactory(provider => provider.GetRequiredService<IMongoClient>());
                o.DatabaseFactory(provider => provider.GetRequiredService<IMongoDatabase>());
                o.UseBusOutbox();
            });
    
            configurator.UsingRabbitMq((_, cfg) =>
            {
                cfg.Host(config["EventBusSettings:HostAddress"]);
            });
        });

        return services;
    }
}