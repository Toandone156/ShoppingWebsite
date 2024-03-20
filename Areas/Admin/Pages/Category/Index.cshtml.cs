using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ShoppingWebsite.Areas.Admin.Pages.Category
{
    [Authorize(Policy = "AdminPolicy")]
    public class IndexModel : PageModel
    {
        private readonly ShoppingWebsite.Data.StoreContext _context;

        public IndexModel(ShoppingWebsite.Data.StoreContext context)
        {
            _context = context;
        }

        public IList<ShoppingWebsite.Models.Category> Category { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Categories != null)
            {
                Category = await _context.Categories.Where(c => !c.IsDelete).ToListAsync();
            }
        }
    }
}
