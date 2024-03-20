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
                TempData["Message"] = "Need login first";
                return Redirect("../index");
            }

            cart = SessionService.GetSessionValue<List<OrderDetail>>(HttpContext, "cart");

            if(cart == null)
            {
                TempData["Message"] = "Cart is empty";
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

        public IActionResult OnPost()
        {
            var id = User.FindFirst(c => c.Type == "Id").Value;

            if (id == null)
            {
                TempData["Message"] = "Need login first";
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
                TotalAmount = TotalAmount()
            };

            SessionService.AddToSession(HttpContext, "cartInfo", order);

            //_context.Orders.Add(order);
            //_context.SaveChanges();

            //var details = SessionService.GetSessionValue<List<OrderDetail>>(HttpContext, "cart");

            //foreach(var item in details)
            //{
            //    item.OrderID = order.OrderID;
            //    item.Product = null;
            //    _context.OrderDetails.Add(item);
            //    _context.SaveChanges();
            //}

            //SessionService.DeleteSession(HttpContext, "cart");

            //TempData["Message"] = "Order successfully!";
            return Redirect("./payment");
        }

        private int TotalAmount()
        {
            var total = 0;

            foreach (var od in cart)
            {
                total += od.Quantity * od.UnitPrice;
            }

            return total;
        }
    }
}
