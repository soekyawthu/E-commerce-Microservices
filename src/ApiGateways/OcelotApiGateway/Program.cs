using Common.Logging;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile($"ocelot.{ builder.Environment.EnvironmentName }.json", true, true);
builder.Host.UseSerilog(SeriLogger.Configure);

builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.AddOcelot()
    .AddCacheManager(x => x.WithDictionaryHandle());

var app = builder.Build();

app.UseRouting();

// Will work with "app.UseEndPoints()" when use "app.UseOcelot()". If not, will not work this url
#pragma warning disable ASP0014
app.UseEndpoints( endpoints => endpoints.MapGet("/", () => "Hello World"));
#pragma warning restore ASP0014


await app.UseOcelot();

app.Run();