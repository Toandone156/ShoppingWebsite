using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using ShoppingWebsite.Services;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace ShoppingWebsite.Pages.Order
{
    [Authorize]
    public class PaymentModel : PageModel
    {
        public string QRUrl { get; set; }
        public int amount { get; set; }

        public async Task<IActionResult> OnGet()
        {
            amount = SessionService.GetSessionValue<Models.Order>(HttpContext, "cartInfo").TotalAmount;
            QRUrl = await GetQRCode(amount);

            if(QRUrl == "fail")
            {
                TempData["Message"] = "Get QR code fail!";
                return RedirectToPage("/checkout");
            }

            return Page();
        }

        async Task<string> GetQRCode(int amount)
        {
            string url = "https://api.vietqr.io/v2/generate"; // Replace with your actual API URL
            string clientId = "2c523b56-5f4d-4309-82df-8e57e2fb0bc5"; // Replace with your client ID
            string apiKey = "d3abe339-eb90-4c49-8466-5003232fb3c1"; // Replace with your API key

            var accountNo = 7010115062002;
            var accountName = "Pham The Toan";
            var addInfo = "id" + User.FindFirstValue("Id");
            var acqId = 970422;
            var template = "compact";

            var jsonBody = new
            {
                accountNo,
                accountName,
                addInfo,
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
