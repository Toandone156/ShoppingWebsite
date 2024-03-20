using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingWebsite.Data;
using ShoppingWebsite.Models;
using ShoppingWebsite.Services;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ShoppingWebsite.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        private readonly StoreContext _context;
        public RegisterModel(StoreContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync([FromForm] string Username, string Fullname, string Password)
        {
            var result = _context.Accounts.Any(acc => acc.Username == Username);

            if (result)
            {
                TempData["Message"] = "Username was exist!";
                return RedirectToPage("../index");
            }

            Account acc = new Account
            {
                Username = Username,
                Password = HashPassword.GetHashPassword(Password),
                FullName = Fullname,
                Type = AccountType.Member
            };

            _context.Accounts.Add(acc);
            _context.SaveChanges();

            TempData["Message"] = "Register success";
            return RedirectToPage("../index");
        }
    }
}
