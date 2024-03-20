using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ShoppingWebsite.Pages.Auth
{
    public class LoginGoogleModel : PageModel
    {
        public async Task OnGet()
        {
            var responseLink = Url.Page("./googleresponse");

            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties()
            {
                RedirectUri = Url.Page("/auth/googleresponse")
            });
        }
    }
}
