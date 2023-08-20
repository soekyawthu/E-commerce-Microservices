using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace Common.Logging;

public class SeriLogger
{
    public static Action<HostBuilderContext, LoggerConfiguration> Configure => (context, configuration) =>
    {
        var elasticsearchUri = context.Configuration.GetValue<string>("ElasticConfiguration:Uri");
        var appName = context.HostingEnvironment.ApplicationName?.ToLower().Replace(".", "-");
        var envName = context.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-");

        configuration
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .WriteTo.Console()
            .WriteTo.Debug()
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticsearchUri!))
            {
                IndexFormat = $"ecommerce-app-logs-{appName}-{envName}-{DateTime.Now:yyyy-MM}",
                AutoRegisterTemplate = true,
                NumberOfShards = 2,
                NumberOfReplicas = 1,
                
            })
            .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName!)
            .Enrich.WithProperty("Application", context.HostingEnvironment.ApplicationName!)
            .ReadFrom.Configuration(context.Configuration);
    };
}