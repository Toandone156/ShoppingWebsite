using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using ShoppingWebsite.Data;
using ShoppingWebsite.Hubs;
using ShoppingWebsite.Models;
using ShoppingWebsite.Services;

namespace ShoppingWebsite.Pages.Order
{
    public class PlaceOrderModel : PageModel
    {
        private StoreContext _context;
        private IHubContext<HubServer> _hub;
        private PartialService _partialService;
        public PlaceOrderModel(StoreContext context, IHubContext<HubServer> hub, PartialService partialService)
        {
            _context = context;
            _hub = hub;
            _partialService = partialService;
        }

        public async Task<IActionResult> OnGet()
        {
            var order = SessionService.GetSessionValue<Models.Order>(HttpContext, "cartInfo");
            order.Account = _context.Accounts.Find(order.AccountID);
            _context.Orders.Add(order);
            _context.SaveChanges();

            var details = SessionService.GetSessionValue<List<Models.OrderDetail>>(HttpContext, "cart");

            foreach (var item in details)
            {
                item.OrderID = order.OrderID;
                item.Product = _context.Products.Find(item.ProductID);
                _context.OrderDetails.Add(item);
                _context.SaveChanges();
            }

            SessionService.DeleteSession(HttpContext, "cartInfo");
            SessionService.DeleteSession(HttpContext, "cart");

            _hub.Clients.Group("Staff").SendAsync("UpdateOrder");

            var placedCustomer = order.Account;
            if (placedCustomer.Username.Contains("@"))
            {
                var receivedMail = placedCustomer.Username;
                var subject = "THÔNG BÁO ĐƠN HÀNG TỪ HOMEMADE CAKE";
                var body = await _partialService.RenderPartialToStringAsync("_EmailTemplatePartial", order);

                MailService.SendMailAsync(receivedMail, subject, body);
            }

            if (placedCustomer.TelegramID != null)
            {
                UpdateStatusMessage updateStatus = new UpdateStatusMessage()
                {
                    OrderID = order.OrderID,
                    OrderStatus = order.status.ToString(),
                    OrderDate = order.OrderDate,
                    Total = order.Details.Sum(d => d.Quantity * d.UnitPrice) + order.ShipAmount
                };

                var sendMessage = await _partialService.RenderPartialToStringAsync("_UpdateStatusPartial", updateStatus);
                TelegramService.SendMessage(order.Account.TelegramID ?? 0, sendMessage);
            }

            TempData["Message"] = "Order successfully!";
            return RedirectToPage("../index");
        }
    }
}
