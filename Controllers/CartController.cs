using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ShoppingWebsite.Data;
using ShoppingWebsite.Models;
using ShoppingWebsite.Services;
using System.Net.Http.Headers;
using System.Text;

namespace ShoppingWebsite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private StoreContext _context;

        public CartController(StoreContext context)
        {
            _context = context;
        }

        [Route("AddToCart")]
        [HttpPost]
        public string AddToCart([FromBody] object data)
        {
            dynamic deData = JsonConvert.DeserializeObject(data.ToString());

            var productid = (int)deData.productid;
            var quantity = (int)deData.quantity;

            _context.ChangeTracker.LazyLoadingEnabled = false;
            var product = _context.Products.SingleOrDefault(p => p.ProductID == productid);
            if (product == default)
            {
                return "Product was not exist";
            }

            List<OrderDetail> cart = SessionService.GetSessionValue<List<OrderDetail>>(HttpContext, "cart") ?? new List<OrderDetail>();

            var cartItem = cart.FirstOrDefault(od => od.ProductID == productid);

            if (cartItem != default)
            {
                if (quantity == 0)
                {
                    cart.Remove(cartItem);
                }
                else
                {
                    cartItem.Quantity = quantity;
                }
            }
            else
            {
                cart.Add(new OrderDetail
                {
                    ProductID = productid,
                    Product = product,
                    UnitPrice = product.UnitPrice,
                    Quantity = quantity
                });
            }

            SessionService.AddToSession(HttpContext, "cart", cart);
            return "Add product success";
        }

        [Route("GetQRCode")]
        [HttpPost]
        public async Task<string> GetQRCode([FromBody] object data)
        {
            dynamic deData = JsonConvert.DeserializeObject(data.ToString());

            var amount = (int)deData.amount;

            string url = "https://api.vietqr.io/v2/generate"; // Replace with your actual API URL
            string clientId = "2c523b56-5f4d-4309-82df-8e57e2fb0bc5"; // Replace with your client ID
            string apiKey = "d3abe339-eb90-4c49-8466-5003232fb3c1"; // Replace with your API key

            var accountNo = 7010115062002;
            var accountName = "Pham The Toan";
            var acqId = 970422;
            var template = "compact";

            var jsonBody = new
            {
                accountNo,
                accountName,
                acqId,
                amount,
                template
            };

            string jsonContent = JsonConvert.SerializeObject(jsonBody);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("x-client-id", clientId);
                client.DefaultRequestHeaders.Add("x-api-key", apiKey);

                var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, stringContent);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<dynamic>(responseContent)?.data.qrDataURL ?? "fail";
                }
            }

            return "fail";
        }
    }
}
