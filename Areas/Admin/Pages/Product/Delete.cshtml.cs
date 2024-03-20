using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShoppingWebsite.Data;
using ShoppingWebsite.Models;

namespace ShoppingWebsite.Areas.Admin.Pages.Product
{
    [Authorize(Policy = "AdminPolicy")]
    public class DeleteModel : PageModel
    {
        public StoreContext _context;

        public DeleteModel(StoreContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }
            var product = _context.Products.Find(id);

            if (product != null)
            {
                product.IsDelete = true;
                _context.SaveChanges();
            }

            TempData["Message"] = "Delete product successfully!";
            return RedirectToPage("./Index");
        }
    }
}
