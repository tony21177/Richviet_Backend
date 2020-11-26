using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Email.Notifier
{
    public class SendGridEmailSender : IEmailSender
    {

        public SendGridEmailSenderOptions Options { get; set; }


        public async Task<Response> SendEmailAsync(SendEmailVo vo)
        {
            return await Execute(Options.ApiKey, vo.Subject, vo.Message, vo.Email);
        }

        public SendGridEmailSender(
            IOptions<SendGridEmailSenderOptions> options
        )
        {
            this.Options = options.Value;
        }

        private async Task<Response> Execute(
            string apiKey,
            string subject,
            string message,
            string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(Options.SenderEmail, Options.SenderName),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            // disable tracking settings
            // ref.: https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);
            msg.SetOpenTracking(false);
            msg.SetGoogleAnalytics(false);
            msg.SetSubscriptionTracking(false);

            return await client.SendEmailAsync(msg);
        }
    }
}