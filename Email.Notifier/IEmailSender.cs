using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Email.Notifier
{
    public interface IEmailSender
    {
         Task SendEmailAsync(SendEmailVo vo);
    }
}
