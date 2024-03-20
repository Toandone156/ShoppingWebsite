using ShoppingWebsite.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace ShoppingWebsite.Services
{
    public interface ICookieAuthentication
    {
        public Task SignInAsync(Account account, HttpContext context);
        public Task SignOutAsync(HttpContext context);
    }

    public class CookieAuthentication : ICookieAuthentication
    {
        public async Task SignInAsync(Account account, HttpContext context)
        {
            AccountType role = account.Type;
            int id = account.AccountID;
            string scheme = "cookie";

            List<Claim> claims = new List<Claim>()
            {
                new Claim("Id", id.ToString()),
                new Claim(ClaimTypes.Name, account.FullName),
                new Claim(ClaimTypes.Role, role.ToString())
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, scheme);

            AuthenticationProperties properties = new AuthenticationProperties()
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddDays(1)
            };

            await context.SignInAsync(scheme,
                new ClaimsPrincipal(claimsIdentity), properties);
        }

        public async Task SignOutAsync(HttpContext context)
        {
            await context.SignOutAsync("cookie");
        }
    }
}
