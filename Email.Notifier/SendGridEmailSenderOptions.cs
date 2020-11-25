namespace Email.Notifier
{
    public class SendGridEmailSenderOptions
    {
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
        public string ApiKey { get; set; }
    }
}