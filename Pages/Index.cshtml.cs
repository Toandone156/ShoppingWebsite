using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ShoppingWebsite.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ShoppingWebsite.Data.StoreContext _context;

        public IndexModel(ShoppingWebsite.Data.StoreContext context)
        {
            _context = context;
        }

        public IList<ShoppingWebsite.Models.Product> Product { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Products != null)
            {
                Product = await _context.Products.Where(p => !p.IsDelete).ToListAsync();
            }
        }
    }
}
