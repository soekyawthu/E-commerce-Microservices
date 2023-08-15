using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TraditionalWebApp.Models;
using TraditionalWebApp.Services;

namespace TraditionalWebApp.Pages
{
    public class CheckOutModel : PageModel
    {
        private readonly IBasketService _basketService;

        public CheckOutModel(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [BindProperty]
        public CheckoutModel? Order { get; set; }

        public BasketModel? Cart { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            const string username = "skt";
            Cart = await _basketService.GetBasket(username);
            return Page();
        }

        public async Task<IActionResult> OnPostCheckOutAsync()
        {
            const string username = "skt";
            var basket = await _basketService.GetBasket(username);

            if (!ModelState.IsValid && basket is null && Order is null)
                return Page();

            Order!.UserName = username;
            Order.TotalPrice = basket!.TotalPrice;

            await _basketService.CheckoutBasket(Order);

            return RedirectToPage("Confirmation", "OrderSubmitted");
        }       
    }
}