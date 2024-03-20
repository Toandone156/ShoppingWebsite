using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingWebsite.Data;
using ShoppingWebsite.Services;

namespace ShoppingWebsite.Pages.Auth
{
    public class LogoutModel : PageModel
    {
        private readonly ICookieAuthentication _cookie;
        public LogoutModel(ICookieAuthentication cookie)
        {
            _cookie = cookie;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await _cookie.SignOutAsync(HttpContext);
            return RedirectToPage("../Index");
        }
    }
}
