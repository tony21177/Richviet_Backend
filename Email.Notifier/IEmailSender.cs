using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Options;

namespace Email.Notifier
{
    public interface IEmailSender
    {
        Task SendEmailAsync(SendEmailVo vo);
    }


    public class SendGridEmailSender : IEmailSender
    {

        public SendGridEmailSenderOptions Options { get; set; }


        public async Task SendEmailAsync(SendEmailVo vo)
        {
            await Execute(Options.ApiKey, vo.Subject, vo.Message, vo.Email);
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

    public class SendEmailVo
    {
        public string Subject { get; set; }
        public string Message { get; set; }
        public string Email { get; set; }
    }

    public class SendGridEmailSenderOptions
    {
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
        public string ApiKey { get; set; }
    }
}
