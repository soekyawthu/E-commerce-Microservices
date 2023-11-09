using Common.Logging;
using MassTransit;
using Ordering.API.CourierActivities;
using Ordering.API.EventBusConsumers;
using Ordering.API.Extensions;
using Ordering.API.StateMachines;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Persistence;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(SeriLogger.Configure);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(configurator =>
{
    configurator.AddConsumer<BasketCheckoutConsumer>();
    configurator.AddConsumer<FulfillOrderConsumer>();
    configurator.AddActivitiesFromNamespaceContaining<ReduceInventoryActivity>();

    configurator.AddSagaStateMachine<OrderStateMachine, OrderState>()
        .MongoDbRepository(r =>
        {
            r.Connection = "mongodb://127.0.0.1:27021";
            r.DatabaseName = "orders";
        });
    
    configurator.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
        cfg.ConfigureEndpoints(ctx);
    });
});

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

app.MigrateDatabase<OrderDbContext>((context, provider) =>
{
    var logger = provider.GetService<ILogger<OrderSeed>>();
    OrderSeed.SeedAsync(logger, context).Wait();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();