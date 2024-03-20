using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ShoppingWebsite.Pages
{
    public class CallModel : PageModel
    {
        [Route("/call/{guid}")]
        public IActionResult OnGet()
        {
            string a = RouteData.Values["guid"].ToString();
            ViewData["id"] = a;
            return Page();
        }
    }
}
