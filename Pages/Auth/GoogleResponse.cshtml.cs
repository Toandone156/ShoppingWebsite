using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShoppingWebsite.Data;
using ShoppingWebsite.Services;
using System.Security.Claims;

namespace ShoppingWebsite.Pages.Auth
{
    public class GoogleResponseModel : PageModel
    {
        private StoreContext _context;
        private ICookieAuthentication _cookie;

        public GoogleResponseModel(StoreContext context, ICookieAuthentication cookie)
        {
            _context = context;
            _cookie = cookie;
        }

        public async Task<IActionResult> OnGet()
        {
            var result = await HttpContext.AuthenticateAsync("cookie");
            var claims = result.Principal;

            var user = _context.Accounts.FirstOrDefault(x => x.Username == claims.FindFirstValue(ClaimTypes.Email));

            if (user == null)
            {
                user = new Models.Account()
                {
                    FullName = claims.FindFirstValue(ClaimTypes.Name),
                    Username = claims.FindFirstValue(ClaimTypes.Email),
                    Type = Models.AccountType.Member
                };

                _context.Add(user);
                await _context.SaveChangesAsync();
            }

            await _cookie.SignInAsync(user, HttpContext);

            return RedirectToPage("../index");
        }
    }
}
