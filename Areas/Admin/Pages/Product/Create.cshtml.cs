using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace ShoppingWebsite.Areas.Admin.Pages.Product
{
    [Authorize(Policy = "AdminPolicy")]
    public class CreateModel : PageModel
    {
        private readonly ShoppingWebsite.Data.StoreContext _context;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _enviroment;

        public CreateModel(ShoppingWebsite.Data.StoreContext context, Microsoft.AspNetCore.Hosting.IHostingEnvironment enviroment)
        {
            _context = context;
            _enviroment = enviroment;
        }

        public IActionResult OnGet()
        {
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "Name");
            return Page();
        }

        [BindProperty]
        public ShoppingWebsite.Models.Product Product { get; set; } = default!;

        [Required(ErrorMessage = "Need one image")]
        [DataType(DataType.Upload)]
        [Display(Name = "Choose product image to upload")]
        [BindProperty]
        public IFormFile[] Files { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            var file = Files.First();
            string fileExtension = Path.GetExtension(file.FileName);
            string newFileName = $"{Guid.NewGuid()}{fileExtension}";
            var fileUploadUrl = Path.Combine(_enviroment.ContentRootPath, "wwwroot\\uploads", newFileName);
            using (var fileStream = new FileStream(fileUploadUrl, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            Product.ProductImage = $"/uploads/{newFileName}";

            if (!ModelState.IsValid || _context.Products == null || Product == null)
            {
                return Page();
            }

            _context.Products.Add(Product);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Create product successfully!";
            return RedirectToPage("./Index");
        }
    }
}
