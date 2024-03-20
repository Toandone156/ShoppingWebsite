using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ShoppingWebsite.Data;

namespace ShoppingWebsite.Areas.Admin.Pages.Category
{
    [Authorize(Policy = "AdminPolicy")]
    public class DeleteModel : PageModel
    {
        private StoreContext _context;
        public DeleteModel(StoreContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }
            var category = _context.Categories.Find(id);

            if (category != null)
            {
                category.IsDelete = true;
                _context.SaveChanges();
            }
            else
            {
                TempData["Message"] = "Category was not exist!";
                return RedirectToPage("./index");
            }

            TempData["Message"] = "Delete category successfully!";
            return RedirectToPage("./Index");
        }
    }
}
