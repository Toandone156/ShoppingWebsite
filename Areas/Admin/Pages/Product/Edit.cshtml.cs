using Castle.Core.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace ShoppingWebsite.Areas.Admin.Pages.Product
{
    [Authorize(Policy = "AdminPolicy")]
    public class EditModel : PageModel
    {
        private readonly ShoppingWebsite.Data.StoreContext _context;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _enviroment;

        public EditModel(ShoppingWebsite.Data.StoreContext context, Microsoft.AspNetCore.Hosting.IHostingEnvironment enviroment)
        {
            _context = context;
            _enviroment = enviroment;
        }

        [BindProperty]
        public ShoppingWebsite.Models.Product Product { get; set; } = default!;

        [Required(ErrorMessage = "Need one image")]
        [DataType(DataType.Upload)]
        [Display(Name = "Choose product image to upload")]
        [BindProperty]
        public IFormFile[] Files { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }
            Product = product;
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!Files.IsNullOrEmpty())
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
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(Product.ProductID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            TempData["Message"] = "Update product successfully!";
            return RedirectToPage("./Index");
        }

        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.ProductID == id)).GetValueOrDefault();
        }
    }
}
