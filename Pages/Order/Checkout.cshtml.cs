using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingWebsite.Data;
using ShoppingWebsite.Models;
using ShoppingWebsite.Services;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Security.Principal;

namespace ShoppingWebsite.Pages.Order
{
    public class CheckoutModel : PageModel
    {
        private StoreContext _context;
        public CheckoutModel(StoreContext context)
        {
            _context = context;
        }

        public List<OrderDetail> cart;

        public IActionResult OnGet()
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["Message"] = "Cần đăng nhập trước khi thanh toán";
                TempData["RequiredLogin"] = true;
                return Redirect("../index");
            }

            cart = SessionService.GetSessionValue<List<OrderDetail>>(HttpContext, "cart");

            if(cart == null)
            {
                TempData["Message"] = "Giỏ hàng trống";
                return RedirectToPage("../index");
            }

            ViewData["Total"] = TotalAmount();

            if (User.Identity.IsAuthenticated)
            {
                var id = int.Parse(User.FindFirstValue("Id"));
                var account = _context.Accounts.Find(id);
                if(account != null)
                {
                    Name = account.FullName;
                    Address = account.Address ?? "";
                    Phone = account.Phone ?? "";
                }
            }

            return Page();
        }

        [BindProperty]
        [Required]
        public string Name { get; set; }

        [BindProperty]
        [Required]
        public string Address { get; set; }

        [BindProperty]
        [Required]
        public string Phone { get; set; }

        [BindProperty]
        public int ShipAmount { get; set; } = 0;

        public IActionResult OnPost()
        {
            var id = User.FindFirst(c => c.Type == "Id").Value;

            if (id == null)
            {
                TempData["Message"] = "Cần đăng nhập trước khi thanh toán";
                return Redirect("../index");
            }

            cart = SessionService.GetSessionValue<List<OrderDetail>>(HttpContext, "cart");

            var account = _context.Accounts.Find(int.Parse(id));


            if (account != null)
            {
                account.Address = Address;
                account.Phone = Phone;
            }

            _context.SaveChanges();

            var order = new Models.Order
            {
                AccountID = Int32.Parse(id),
                OrderDate = DateTime.Now,
                ShipAddress = Address,
                Phone = Phone,
                ShipAmount = ShipAmount,
                TotalAmount = TotalAmount() + ShipAmount
            };

            SessionService.AddToSession(HttpContext, "cartInfo", order);
            return Redirect("./payment");
        }

        private int TotalAmount()
        {
            return cart.Sum(d => d.Quantity * d.UnitPrice);
        }
    }
}
