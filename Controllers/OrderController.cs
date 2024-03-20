using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using ShoppingWebsite.Data;
using ShoppingWebsite.Hubs;

namespace ShoppingWebsite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private StoreContext _context;
        private IHubContext<HubServer> _hub;
        public OrderController(StoreContext context, IHubContext<HubServer> hub)
        {
            _context = context;
            _hub = hub;
        }

        [Route("UpdateStatus")]
        [HttpPost]
        public string UpdateStatus([FromBody] object data)
        {
            dynamic deData = JsonConvert.DeserializeObject<dynamic>(data.ToString());
            var orderId = (int)deData.id;

            var order = _context.Orders.Find(orderId);
            if (order == null) return "";

            order.status += 1;

            _context.SaveChanges();

            _hub.Clients.Group(order.AccountID.ToString()).SendAsync("UpdateStatus");

            return order.status.ToString();
        }
    }
}
