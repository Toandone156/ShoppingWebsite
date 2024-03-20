using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ShoppingWebsite.Areas.Admin.Pages.Category
{
    [Authorize(Policy = "AdminPolicy")]
    public class CreateModel : PageModel
    {
        private readonly ShoppingWebsite.Data.StoreContext _context;

        public CreateModel(ShoppingWebsite.Data.StoreContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public ShoppingWebsite.Models.Category Category { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.Categories == null || Category == null)
            {
                return Page();
            }

            _context.Categories.Add(Category);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Create new category successfully!";
            return RedirectToPage("./Index");
        }
    }
}
