using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingWebsite.Data;
using ShoppingWebsite.Services;
using System.ComponentModel.DataAnnotations;

namespace ShoppingWebsite.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly StoreContext _context;
        private readonly ICookieAuthentication _cookie;
        public LoginModel(StoreContext context, ICookieAuthentication cookie)
        {
            _context = context;
            _cookie = cookie;
        }

        public async Task<IActionResult> OnPostAsync(string Username, string Password)
        {
            if (ModelState.IsValid)
            {
                var account = _context.Accounts.SingleOrDefault(acc => acc.Username == Username && acc.Password == HashPassword.GetHashPassword(Password));
                if (account == default)
                {
                    TempData["Message"] = "Username or Password is wrong";
                    return Page();
                }

                await _cookie.SignInAsync(account, HttpContext);
            }

            if(SessionService.GetSessionValue<long>(HttpContext, "TelegramID") != null)
            {
                return RedirectToPage("../AddTelegramNoti");
            }

            return RedirectToPage("../Index");
        }
    }
}
