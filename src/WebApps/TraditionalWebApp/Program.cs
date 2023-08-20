using Common.Logging;
using Serilog;
using TraditionalWebApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(SeriLogger.Configure);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddHttpClient<ICatalogService, CatalogService>(client => 
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:GatewayUrl"]!));

builder.Services.AddHttpClient<IBasketService, BasketService>(client => 
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:GatewayUrl"]!));

builder.Services.AddHttpClient<IOrderService, OrderService>(client => 
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:GatewayUrl"]!));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();