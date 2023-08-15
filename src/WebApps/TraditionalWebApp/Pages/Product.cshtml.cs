using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TraditionalWebApp.Models;
using TraditionalWebApp.Services;

namespace TraditionalWebApp.Pages
{
    public class ProductModel : PageModel
    {
        private readonly ICatalogService _catalogService;
        private readonly IBasketService _basketService;

        public ProductModel(ICatalogService catalogService, IBasketService basketService)
        {
            _catalogService = catalogService;
            _basketService = basketService;
        }

        public IEnumerable<string> CategoryList { get; set; } = new List<string>();
        public IEnumerable<CatalogModel> ProductList { get; set; } = new List<CatalogModel>();
        
        
        [BindProperty(SupportsGet = true)]
        public string? SelectedCategory { get; set; }

        public async Task<IActionResult> OnGetAsync(string category)
        {
            ProductList = await _catalogService.GetCatalog();
            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(string productId)
        {
            //if (!User.Identity.IsAuthenticated)
            //    return RedirectToPage("./Account/Login", new { area = "Identity" });

            const string username = "skt";
            var product = await _catalogService.GetCatalog(productId);
            var basket = await _basketService.GetBasket(username);

            if (product is null || basket is null)
                return Page();
            
            basket.Items.Add(new BasketItemModel
            {
                Color = "Black",
                ProductId = product.Id,
                ProductName = product.Name,
                Quantity = 1,
                Price = product.Price
            });

            await _basketService.UpdateBasket(basket);
            
            return RedirectToPage("Cart");
        }
    }
}