using MailKit.Security;
using MimeKit;

namespace ShoppingWebsite.Services
{
    public class MailService
    {
        const string DisplayName = "HomemadeCake_AUTOMAIL";
        const string Email = "toanpt.developer@gmail.com";
        const string Host = "smtp.gmail.com";
        const int Port = 587;
        const string Password = "nynhxauhjzwuuoda";

        public static async Task SendMailAsync(string receiveMail, string subject, string body)
        {
            var email = new MimeMessage();
            email.Sender = new MailboxAddress(DisplayName, Email);
            email.From.Add(new MailboxAddress(DisplayName, Email));
            email.To.Add(MailboxAddress.Parse(receiveMail));
            email.Subject = subject;


            var builder = new BodyBuilder();
            builder.HtmlBody = body;
            email.Body = builder.ToMessageBody();

            // dùng SmtpClient của MailKit
            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                await smtp.ConnectAsync(Host, Port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(Email, Password);
                await smtp.SendAsync(email);
            }
            catch (Exception ex)
            {
                //_logger.LogInformation("Send mail fail");
                //_logger.LogError(ex.Message);
            }

            await smtp.DisconnectAsync(true);
        }
    }
}
