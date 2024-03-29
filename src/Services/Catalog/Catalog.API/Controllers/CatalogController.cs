using System.Net;
using Catalog.API.Entities;
using Catalog.API.Models;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CatalogController : ControllerBase
{
    private readonly ILogger<CatalogController> _logger;
    private readonly IProductRepository _repository;

    public CatalogController(ILogger<CatalogController> logger, IProductRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        var products = await _repository.GetProducts();
        return Ok(products);
    }

    [HttpGet("{id:guid}", Name = "GetProduct")]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Product>> GetProductById(Guid id)
    {
        var product = await _repository.GetProduct(id);
        if (product is not null) return Ok(product);
        
        _logger.LogError("Product with id: {Id}, not found", id);
        return NotFound();
    }
    
    [Route("[action]/{name}", Name = "GetProductByName")]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductByName(string name)
    {
        var products = await _repository.GetProductByName(name);
        return Ok(products);
    }

    [Route("[action]/{category}", Name = "GetProductByCategory")]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string category)
    {
        var products = await _repository.GetProductByCategory(category);
        return Ok(products);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ProductViewModel>> CreateProduct([FromForm] Product product)
    {
        if (product.ImageFile is not { Length: > 0 }) return BadRequest();
        await _repository.CreateProduct(product);
        return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
    }

    [HttpPut]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> UpdateProduct([FromBody] Product product)
    {
        return Ok(await _repository.UpdateProduct(product));
    }

    [HttpDelete("{id:guid}", Name = "DeleteProduct")]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteProductById(Guid id)
    {
        return await _repository.DeleteProduct(id) ? NoContent() : BadRequest();
    }
}