using TraditionalWebApp.Extensions;
using TraditionalWebApp.Models;

namespace TraditionalWebApp.Services;

public class CatalogService : ICatalogService
{
    private readonly ILogger<CatalogService> _logger;
    private readonly HttpClient _httpClient;

    public CatalogService(ILogger<CatalogService> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }
    
    public async Task<IEnumerable<CatalogModel>> GetCatalog()
    {
        try
        {
            var response = await _httpClient.GetAsync("Catalog");
            _logger.LogInformation("Fetched Catalog API Property - {Property}, Url - {Url}", 6, _httpClient.BaseAddress);
            return await response.ReadContentAs<List<CatalogModel>>() ?? new List<CatalogModel>();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<CatalogModel?> GetCatalog(string id)
    {
        var response = await _httpClient.GetAsync($"Catalog/{id}");
        return await response.ReadContentAs<CatalogModel>();
    }

    public async Task<IEnumerable<CatalogModel>> GetCatalogByCategory(string category)
    {
        var response = await _httpClient.GetAsync($"Catalog/GetProductByCategory/{category}");
        return await response.ReadContentAs<List<CatalogModel>>() ?? new List<CatalogModel>();
    }
}