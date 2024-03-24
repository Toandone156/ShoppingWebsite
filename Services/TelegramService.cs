using Telegram.Bot;

namespace ShoppingWebsite.Services
{
    public class UpdateStatusMessage
    {
        public int OrderID { get; set; }
        public string OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public int Total { get; set; }
    }

    public class TelegramService
    {
        static string ApiKey = "7066399079:AAEevcdEuvryJGn695WNmGEv8cPBfx6t_K0";

        public static async Task SendMessage(long receivedID, string message)
        {
            TelegramBotClient client = new TelegramBotClient(ApiKey);
            await client.SendTextMessageAsync(receivedID, message, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
        }
    }
}
