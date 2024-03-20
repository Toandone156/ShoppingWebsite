using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;
using System.Security.Claims;
using ShoppingWebsite.Data;

namespace ShoppingWebsite.Hubs
{
    public class HubServer : Hub
    {
        private StoreContext _context;
        public HubServer(StoreContext context)
        {
            _context = context;
        }

        public async Task JoinRoom()
        {
            if (!Context.User.Identity.IsAuthenticated) return;

            var type = Context.User.FindFirstValue(ClaimTypes.Role).ToString();

            Groups.AddToGroupAsync(Context.ConnectionId, Context.User.FindFirstValue("Id"));

            if (type == "Staff")
            {
                Groups.AddToGroupAsync(Context.ConnectionId, "Staff");
            }
        }

        public async Task CallStaff(string userId)
        {
            var user = _context.Accounts.Find(int.Parse(userId));

            if (user == null) return;

            var name = user.FullName;

            Clients.Group("Staff").SendAsync("NewCall", userId, name);
        }

        public async Task ReceivedCall(string userId)
        {
            var guid = Guid.NewGuid().ToString();

            var accId = Context.User.FindFirstValue("Id");
            await Clients.Group(accId).SendAsync("MakeCall", guid);
            Thread.Sleep(2000);
            await Clients.Group(userId).SendAsync("MakeCall", guid);
        }
    }
}
