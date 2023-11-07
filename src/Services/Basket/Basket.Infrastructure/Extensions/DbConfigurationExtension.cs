using System.Linq.Expressions;
using MassTransit.MongoDbIntegration.Saga;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Basket.Infrastructure.Extensions;

public static class DbConfigurationExtension
{
    public static IServiceCollection AddMongoDbCollection<T>(this IServiceCollection services, Expression<Func<T, Guid>> idPropertyExpression)
        where T : class
    {
        services.TryAddSingleton(MongoDbCollectionFactory);
        
        return services;

        IMongoCollection<T> MongoDbCollectionFactory(IServiceProvider provider)
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(T)))
            {
                BsonClassMap.RegisterClassMap(new BsonClassMap<T>(cfg =>
                {
                    cfg.AutoMap();
                    cfg.MapIdProperty(idPropertyExpression);
                }));
            }

            var database = provider.GetRequiredService<IMongoDatabase>();
            var collectionNameFormatter = DotCaseCollectionNameFormatter.Instance;

            return database.GetCollection<T>(collectionNameFormatter.Collection<T>());
        }
    }
}