using Catalog.API.Data;
using Catalog.API.Entities;
using Catalog.API.Utils;
using MongoDB.Driver;

namespace Catalog.API.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ICatalogContext _context;
    private readonly IWebHostEnvironment _environment;
    private readonly string _baseUrl;

    public ProductRepository(ICatalogContext context, IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _environment = environment;
        var request = httpContextAccessor.HttpContext!.Request;
        _baseUrl = $"{request.Scheme}://{request.Host}{request.PathBase}";
    }
    
    public async Task<IEnumerable<Product>> GetProducts()
    {
        return await _context.Products
            .Find(p => true)
            .ToListAsync();
    }

    public async Task<Product?> GetProduct(Guid id)
    {
        return await _context.Products
            .Find(p => p.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Product>> GetProductByName(string name)
    {
        var filter = Builders<Product>.Filter.Eq(p => p.Name, name);
        return await _context.Products
            .Find(filter)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
    {
        var filter = Builders<Product>.Filter.Eq(p => p.Category, categoryName);

        return await _context
            .Products
            .Find(filter)
            .ToListAsync();
    }

    public async Task CreateProduct(Product product)
    {
        product.Id = Guid.NewGuid();
        if (!Directory.Exists(_environment.WebRootPath + "\\images")) {
            Directory.CreateDirectory(_environment.WebRootPath + @"\images\");
        }

        var fileName = product.Id + Path.GetExtension(product.ImageFile?.FileName);
        var filePath = _environment.WebRootPath + @"\images\" + fileName;
        await using var filestream = File.Create(filePath);
        await product.ImageFile!.CopyToAsync(filestream);
        filestream.Flush();
        
        product.Image = $"{_baseUrl}/images/{fileName}";
        await _context.Products.InsertOneAsync(product);
    }

    public async Task<bool> UpdateProduct(Product product)
    {
        var updateResult = await _context.Products
            .ReplaceOneAsync(p => p.Id == product.Id, product);

        return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
    }

    public async Task<bool> DeleteProduct(Guid id)
    {
        var product = await GetProduct(id);

        if (product is null) return false;
        
        var imageFileName = ImageUrlProcessor.GetFileNameFromUrl(product.Image!);
        var filePath = Path.Combine(_environment.WebRootPath, "images", imageFileName!);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
            
        var filter = Builders<Product>.Filter.Eq(p => p.Id, id);

        var deleteResult = await _context
            .Products
            .DeleteOneAsync(filter);

        return deleteResult.IsAcknowledged
               && deleteResult.DeletedCount > 0;

    }
}