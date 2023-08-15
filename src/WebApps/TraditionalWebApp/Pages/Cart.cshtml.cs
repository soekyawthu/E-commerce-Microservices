using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TraditionalWebApp.Models;
using TraditionalWebApp.Services;

namespace TraditionalWebApp.Pages
{
    public class CartModel : PageModel
    {
        private readonly IBasketService _basketService;

        public CartModel(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public BasketModel? Cart { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            const string username = "skt";
            Cart = await _basketService.GetBasket(username);
            return Page();
        }

        public async Task<IActionResult> OnPostRemoveToCartAsync(string productId)
        {
            const string username = "skt";
            var basket = await _basketService.GetBasket(username);
            var item = basket?.Items.FirstOrDefault(x => x.ProductId == productId);

            if (item is null) return RedirectToPage();
            
            basket?.Items.Remove(item);
            await _basketService.UpdateBasket(basket!);

            return RedirectToPage();
        }
    }
}