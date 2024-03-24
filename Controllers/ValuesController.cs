using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShoppingWebsite.Models;
using ShoppingWebsite.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ShoppingWebsite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        readonly PartialService _partialService;
        public ValuesController(PartialService partialService)
        {
            _partialService = partialService;
        }

        [Route("Test")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] object data)
        {
            var updateMessage = JsonConvert.DeserializeObject<Update>(data.ToString());

            if(updateMessage != null && updateMessage.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                long senderID = updateMessage.Message.From.Id;
                string message = updateMessage.Message.Text;
                string sendMessage = "";
                switch (message)
                {
                    case "/start":
                        sendMessage = await _partialService.RenderPartialToStringAsync<object>("_IntroducePartial", null);
                        TelegramService.SendMessage(senderID, sendMessage);
                        break;
                    case "/login":
                        var token = JWTService.GenerateToken(senderID.ToString());

                        var location = new Uri($"{Request.Scheme}://{Request.Host}");
                        var url = location.AbsoluteUri + $"addtelegramnoti?token={token}";

                        sendMessage = await _partialService.RenderPartialToStringAsync("_TokenTelegramPartial", url);

                        TelegramService.SendMessage(senderID, sendMessage);
                        break;
                    default:
                        TelegramService.SendMessage(senderID, "<b>Cảm ơn bạn đã gửi tin nhắn cho chúng tôi. Nhân viên phụ trách sẽ hỗ trợ bạn sau ít phút</b>");
                        break;
                }
            }

            return Ok();
        }
    }
}
