using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TraditionalWebApp.Models;
using TraditionalWebApp.Services;

namespace TraditionalWebApp.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ICatalogService _catalogService;
    private readonly IBasketService _basketService;

    public IndexModel(ILogger<IndexModel> logger, ICatalogService catalogService, IBasketService basketService)
    {
        _logger = logger;
        _catalogService = catalogService;
        _basketService = basketService;
    }

    public IEnumerable<CatalogModel> ProductList { get; set; } = new List<CatalogModel>();

    public async Task<IActionResult> OnGet()
    {
        ProductList = await _catalogService.GetCatalog();
        return Page();
    }
    
    public async Task<IActionResult> OnPostAddToCartAsync(string productId)
    {
        var product = await _catalogService.GetCatalog(productId);

        const string userName = "swn";
        var basket = await _basketService.GetBasket(userName);

        if (basket is null || product is null)
            return Page();

        basket.Items.Add(new BasketItemModel
        {
            ProductId = productId,
            ProductName = product.Name,
            Price = product.Price,
            Quantity = 1,
            Color = "Black"
        });

        await _basketService.UpdateBasket(basket);
        return RedirectToPage("Cart");
    }
}