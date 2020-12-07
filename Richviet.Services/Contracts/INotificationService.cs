using Frontend.DB.EF.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Richviet.Services.Contracts
{
    public interface INotificationService
    {
        void SendPush(string mobileToken, string title, string body);

        PushNotificationSetting UpdateMobileToken(int userId, string mobileToken);

        PushNotificationSetting SwitchNotification(int userId, bool switchFlag);

        PushNotificationSetting GetNotificationState(int userId);
    }
}
