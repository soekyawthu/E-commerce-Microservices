using EventBus.Messages.Commands;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.API.StateMachines;
using Ordering.Application.Consumers;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.CourierActivities;
using Ordering.Application.Models;
using Ordering.Application.StateMachines;
using Ordering.Infrastructure.Mail;
using Ordering.Infrastructure.Repositories;

namespace Ordering.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        /*
        services.AddDbContext<OrderDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("OrderingConnectionString")));
            */
        
        services.AddMassTransit(configurator =>
        {
            configurator.SetKebabCaseEndpointNameFormatter();
            configurator.AddRequestClient<CheckOrder>();
            configurator.AddRequestClient<UpdateOrder>();
            configurator.AddRequestClient<DeleteOrder>();
            configurator.AddConsumer<FulfillOrderConsumer>();
            configurator.AddActivitiesFromNamespaceContaining<ReduceInventoryActivity>();
            configurator.AddActivitiesFromNamespaceContaining<PaymentActivity>();

            configurator.AddSagaStateMachine<OrderStateMachine, OrderState, OrderStateDefinition>()
                .MongoDbRepository(r =>
                {
                    r.Connection = "mongodb://127.0.0.1:27021";
                    r.DatabaseName = "orders";
                });
    
            configurator.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(configuration["EventBusSettings:HostAddress"]);
                cfg.ConfigureEndpoints(ctx);
            });
        });


        services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
        services.AddScoped<IOrderRepository, OrderRepository>();

        services.Configure<EmailSettings>(_ => configuration.GetSection("EmailSettings"));
        services.AddTransient<IEmailService, EmailService>();
        services.AddFluentEmail(string.Empty, string.Empty);
        return services;
    }
}