using TraditionalWebApp.Models;

namespace TraditionalWebApp.Services;

public interface ICatalogService
{
    public Task<IEnumerable<CatalogModel>> GetCatalog();
    public Task<CatalogModel?> GetCatalog(string id);
    public Task<IEnumerable<CatalogModel>> GetCatalogByCategory(string category);
}