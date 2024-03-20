using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ShoppingWebsite.Areas.Admin.Pages
{
    [Authorize(Policy = "AdminPolicy")]
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            return Page();
        }
    }
}
