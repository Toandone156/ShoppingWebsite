using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingWebsite.Models;
using ShoppingWebsite.Services;

namespace ShoppingWebsite.Pages.Order
{
    public class CartModel : PageModel
    {
        public List<OrderDetail> Cart { get; set; }

        public IActionResult OnGet()
        {
            Cart = SessionService.GetSessionValue<List<OrderDetail>>(HttpContext, "cart") ?? new List<OrderDetail>();
            if (Cart.Count > 0)
            {
                ViewData["Total"] = TotalAmount();
            }
            return Page();
        }

        private int TotalAmount()
        {
            var total = 0;

            foreach(var od in Cart)
            {
                total += od.Quantity * od.UnitPrice;
            }

            return total;
        }
    }
}
