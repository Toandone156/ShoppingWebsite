using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShoppingWebsite.Data;

namespace ShoppingWebsite.Pages
{
    public class PrivacyModel : PageModel
    {
        private StoreContext _context;

        public PrivacyModel(StoreContext context)
        {
            _context = context;
        }

        public List<Models.Product> Products { get; set; }

        public IActionResult OnGet()
        {
            Products = _context.Products.Where(p => !p.IsDelete).ToList(); 
            ViewData["CategoryID"] = new SelectList(_context.Categories.Where(c => !c.IsDelete), "CategoryID", "Name");
            return Page();
        }
    }

}
