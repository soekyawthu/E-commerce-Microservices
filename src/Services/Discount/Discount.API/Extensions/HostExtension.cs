using Npgsql;
using Polly;
using Serilog;

namespace Discount.API.Extensions;

public static class HostExtension
{
    public static IHost MigrateDatabase<TContext>(this IHost host) 
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var configuration = services.GetRequiredService<IConfiguration>();
        var logger = services.GetRequiredService<ILogger<TContext>>();

        try
        {
            logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext).Name);

            var retry = Policy
                .Handle<NpgsqlException>()
                .WaitAndRetry(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (ex, timespan, retryCount, arg4) =>
                    {
                        Log.Error("Retry {RetryCount} of {PolicyKey} at {OperationKey} & {Timespan}, due to: {Result}",
                            retryCount, arg4.PolicyKey, arg4.OperationKey, timespan, ex.Message);
                    });
            
            retry.Execute(() => ExecuteMigrations(configuration));

            logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TContext).Name);
        }
        catch (NpgsqlException ex)
        {
            logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(TContext).Name);
        }

        return host;
    }

    private static void ExecuteMigrations(IConfiguration configuration)
    {
        using var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        connection.Open();

        using var command = new NpgsqlCommand();
        command.Connection = connection;

        command.CommandText = "DROP TABLE IF EXISTS Coupon";
        command.ExecuteNonQuery();

        command.CommandText = @"CREATE TABLE Coupon(Id SERIAL PRIMARY KEY, 
                                                                ProductName VARCHAR(24) NOT NULL,
                                                                Description TEXT,
                                                                Amount INT)";
        command.ExecuteNonQuery();


        command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('IPhone X', 'IPhone Discount', 150);";
        command.ExecuteNonQuery();

        command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('Samsung 10', 'Samsung Discount', 100);";
        command.ExecuteNonQuery();
    }        
}