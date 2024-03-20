using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ShoppingWebsite.Areas.Admin.Pages.Order
{
    [Authorize(Policy = "AdminPolicy")]
    public class DetailsModel : PageModel
    {
        private readonly ShoppingWebsite.Data.StoreContext _context;

        public DetailsModel(ShoppingWebsite.Data.StoreContext context)
        {
            _context = context;
        }

        public ShoppingWebsite.Models.Order Order { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FirstOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }
            else
            {
                Order = order;
            }
            return Page();
        }
    }
}
