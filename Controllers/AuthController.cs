using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using ShoppingWebsite.Data;
using ShoppingWebsite.Hubs;
using ShoppingWebsite.Migrations;
using ShoppingWebsite.Services;

namespace ShoppingWebsite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private StoreContext _context;
        private readonly IHubContext<HubServer> _hub;
        public AuthController(StoreContext context, IHubContext<HubServer> hub)
        {
            _context = context;
            _hub = hub;
        }

        [Route("CheckLogin")]
        [HttpPost]
        public bool CheckLogin([FromBody] object data)
        {
            dynamic deData = JsonConvert.DeserializeObject(data.ToString());
            var email = (string) deData.email;
            var password = (string) deData.password;

            var check = _context.Accounts.Any(acc => acc.Username == email && acc.Password == HashPassword.GetHashPassword(password));
            return check;
        }

        [Route("CheckEmail")]
        [HttpPost]
        public bool CheckEmail([FromBody] object data)
        {
            dynamic deData = JsonConvert.DeserializeObject(data.ToString());
            var email = (string)deData.email;

            var account = _context.Accounts.SingleOrDefault(acc => acc.Username == email);
            return account != null;
        }

        [Route("CassoNoti")]
        [HttpPost]
        public bool CassoNoti([FromBody] object data)
        {
            dynamic deData = JsonConvert.DeserializeObject<dynamic>(data.ToString());
            var amount = (int) deData.data[0].amount;
            var desc = (string)deData.data[0].description;

            string? idValue = desc.Split(' ').FirstOrDefault(x => x.StartsWith("id"))?.Substring(2);

            if (idValue == null) return true;

            var result = int.TryParse(idValue, out int id);

            if (result)
            {
                _hub.Clients.Groups(id.ToString()).SendAsync("ReceiveMoney", "Payment successfully!", amount);
            }

            return true;
        }
    }
}
