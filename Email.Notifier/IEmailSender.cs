using SendGrid;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Email.Notifier
{
    public interface IEmailSender
    {
         Task<Response> SendEmailAsync(SendEmailVo vo);
    }
}
