using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingWebsite.Data;
using System.Security.Claims;

namespace ShoppingWebsite.Pages.Bill
{
    public class BillHistoryModel : PageModel
    {
        private StoreContext _context;
        public BillHistoryModel(StoreContext context)
        {
            _context = context;
        }

        public List<Models.Order> MyOrders { get; set; }

        public IActionResult OnGet()
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["Message"] = "Need login first";
                return RedirectToPage("../index");
            }

            var id = int.Parse(User.FindFirstValue("Id"));

            MyOrders = _context.Accounts.Find(id)?.Orders?.OrderByDescending(o => o.OrderDate).ToList() ?? new List<Models.Order>();

            foreach(var order in MyOrders)
            {
                order.TotalAmount = TotalAmount(order) + order.ShipAmount;
            }

            return Page();
        }

        private int TotalAmount(Models.Order order)
        {
            var total = 0;

            foreach (var od in order.Details)
            {
                total += od.Quantity * od.UnitPrice;
            }

            return total;
        }

        public PartialViewResult OnGetListBills()
        {
            var id = int.Parse(User.FindFirstValue("Id"));

            MyOrders = _context.Accounts.Find(id)?.Orders?.OrderByDescending(o => o.OrderDate).ToList() ?? new List<Models.Order>();

            foreach (var order in MyOrders)
            {
                order.TotalAmount = TotalAmount(order);
            }

            return Partial("_OrderPartial", MyOrders);
        }
    }
}
