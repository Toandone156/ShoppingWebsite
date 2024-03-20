using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ShoppingWebsite.Areas.Admin.Pages.Category
{
    [Authorize(Policy = "AdminPolicy")]
    public class EditModel : PageModel
    {
        private readonly ShoppingWebsite.Data.StoreContext _context;

        public EditModel(ShoppingWebsite.Data.StoreContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ShoppingWebsite.Models.Category Category { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FirstOrDefaultAsync(m => m.CategoryID == id);
            if (category == null)
            {
                return NotFound();
            }
            Category = category;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(Category.CategoryID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            TempData["Message"] = "Update category successfully!";
            return RedirectToPage("./Index");
        }

        private bool CategoryExists(int id)
        {
            return (_context.Categories?.Any(e => e.CategoryID == id)).GetValueOrDefault();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }
            var category = _context.Categories.Find(id);

            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }

            TempData["Message"] = "Delete category successfully!";
            return RedirectToPage("./Index");
        }
    }
}
