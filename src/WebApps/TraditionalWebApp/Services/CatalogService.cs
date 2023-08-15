using TraditionalWebApp.Extensions;
using TraditionalWebApp.Models;

namespace TraditionalWebApp.Services;

public class CatalogService : ICatalogService
{
    private readonly HttpClient _httpClient;

    public CatalogService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<IEnumerable<CatalogModel>> GetCatalog()
    {
        var response = await _httpClient.GetAsync("Catalog");
        return await response.ReadContentAs<List<CatalogModel>>() ?? new List<CatalogModel>();
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