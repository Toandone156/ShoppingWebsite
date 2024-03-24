using Castle.Core.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingWebsite.Data;
using ShoppingWebsite.Services;
using System.Security.Claims;

namespace ShoppingWebsite.Pages
{
    public class AddTelegramNotiModel : PageModel
    {
        readonly StoreContext _context;
        public AddTelegramNotiModel(StoreContext context)
        {
            _context = context;
        }

        public IActionResult OnGet([FromQuery] string? token)
        {
            var tokenTelegramID = JWTService.ValidateToken(token);
            var sessionTelegramID = SessionService.GetSessionValue<long?>(HttpContext, "TelegramID");
            if(tokenTelegramID.IsNullOrEmpty() && sessionTelegramID == null)
            {
                TempData["Message"] = "Token không chính xác";
                return RedirectToPage("Index");
            }

            long telegramID = sessionTelegramID ?? long.Parse(tokenTelegramID);

            if (User.Identity.IsAuthenticated)
            {
                var id = int.Parse(User.FindFirstValue("Id"));
                var account = _context.Accounts.Find(id);
                account.TelegramID = telegramID;
                _context.SaveChanges();
                SessionService.DeleteSession(HttpContext, "TelegramID");
                TelegramService.SendMessage(telegramID, $"<b>Thêm tài khoản thành công. Bạn sẽ nhận được thông báo khi có cập nhật đơn hàng từ tài khoản <i>{account.FullName}</i>.</b>");
                TempData["Message"] = "Thêm telegram vào tài khoản thành công";
            }
            else
            {
                SessionService.AddToSession(HttpContext, "TelegramID", telegramID);
                TempData["Message"] = "Cần đăng nhập để thêm telegram";
                TempData["RequiredLogin"] = true;
            }

            return RedirectToPage("Index");
        }
    }
}
