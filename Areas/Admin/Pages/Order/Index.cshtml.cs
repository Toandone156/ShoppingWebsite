using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ShoppingWebsite.Areas.Admin.Pages.Order
{
    public class IndexModel : PageModel
    {
        private readonly ShoppingWebsite.Data.StoreContext _context;

        public IndexModel(ShoppingWebsite.Data.StoreContext context)
        {
            _context = context;
        }

        public IList<ShoppingWebsite.Models.Order> Order { get; set; } = default!;

        [Authorize(Policy = "AdminPolicy")]
        public async Task OnGetAsync()
        {
            if (_context.Orders != null)
            {
                var userId = int.Parse(User.FindFirst("Id").Value);
                var role = User.FindFirst(ClaimTypes.Role).Value;

                if (role == "Staff")
                {
                    Order = await _context.Orders
                    .Include(o => o.Account).OrderByDescending(o => o.OrderID).ToListAsync();
                }
                else
                {
                    Order = await _context.Orders.Where(o => o.AccountID == userId).ToListAsync();
                }

            }
        }
        
        public PartialViewResult OnGetListOrders()
        {
            Order = _context.Orders
            .Include(o => o.Account).OrderByDescending(o => o.OrderID).ToList();

            return Partial("_OrderPartial", Order);
        }
    }
}
