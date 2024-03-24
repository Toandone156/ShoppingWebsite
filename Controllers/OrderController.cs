using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using ShoppingWebsite.Data;
using ShoppingWebsite.Hubs;
using ShoppingWebsite.Services;

namespace ShoppingWebsite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private StoreContext _context;
        private IHubContext<HubServer> _hub;
        private PartialService _partialService;
        public OrderController(StoreContext context, IHubContext<HubServer> hub, PartialService partialService)
        {
            _context = context;
            _hub = hub;
            _partialService = partialService;
        }

        [Route("UpdateStatus")]
        [HttpPost]
        public async Task<string> UpdateStatus([FromBody] object data)
        {
            dynamic deData = JsonConvert.DeserializeObject<dynamic>(data.ToString());
            var orderId = (int)deData.id;

            var order = _context.Orders.Find(orderId);
            if (order == null) return "";

            order.status += 1;

            _context.SaveChanges();

            _hub.Clients.Group(order.AccountID.ToString()).SendAsync("UpdateStatus");

            if(order.Account.TelegramID != null)
            {
                UpdateStatusMessage updateStatus = new UpdateStatusMessage()
                {
                    OrderID = order.OrderID,
                    OrderStatus = order.status.ToString(),
                    OrderDate = order.OrderDate,
                    Total = order.Details.Sum(d => d.Quantity * d.UnitPrice)
                };

                var sendMessage = await _partialService.RenderPartialToStringAsync("_UpdateStatusPartial", updateStatus);
                TelegramService.SendMessage(order.Account.TelegramID ?? 0, sendMessage);
            }

            return order.status.ToString();
        }
    }
}
