using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using ShoppingWebsite.Data;
using ShoppingWebsite.Hubs;
using ShoppingWebsite.Services;

namespace ShoppingWebsite.Pages.Order
{
    public class PlaceOrderModel : PageModel
    {
        private StoreContext _context;
        private IHubContext<HubServer> _hub;
        public PlaceOrderModel(StoreContext context, IHubContext<HubServer> hub)
        {
            _context = context;
            _hub = hub;
        }

        public IActionResult OnGet()
        {
            var order = SessionService.GetSessionValue<Models.Order>(HttpContext, "cartInfo");
            _context.Orders.Add(order);
            _context.SaveChanges();

            var details = SessionService.GetSessionValue<List<Models.OrderDetail>>(HttpContext, "cart");

            foreach (var item in details)
            {
                item.OrderID = order.OrderID;
                item.Product = null;
                _context.OrderDetails.Add(item);
                _context.SaveChanges();
            }

            SessionService.DeleteSession(HttpContext, "cart");

            _hub.Clients.Group("Staff").SendAsync("UpdateOrder");

            TempData["Message"] = "Order successfully!";
            return RedirectToPage("../index");
        }
    }
}
