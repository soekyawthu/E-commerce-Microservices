using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TraditionalWebApp.Models;
using TraditionalWebApp.Services;

namespace TraditionalWebApp.Pages
{
    public class OrderModel : PageModel
    {
        private readonly IOrderService _orderService;

        public OrderModel(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public IEnumerable<OrderResponseModel> Orders { get; set; } = new List<OrderResponseModel>();

        public async Task<IActionResult> OnGetAsync()
        {
            //Orders = await _orderRepository.GetOrdersByUserName("test");
            const string username = "skt";
            Orders = await _orderService.GetOrdersByUsername(username);
            return Page();
        }       
    }
}