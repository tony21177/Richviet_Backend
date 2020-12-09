using Frontend.DB.EF.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Richviet.Services.Contracts
{
    public interface INotificationService
    {
        Task<bool> SendNotification(int userId, string title, string content);

        bool SaveNotificationMessage(int userId, string title, string content, string language);

        Task<bool> SaveAndSendNotification(int userId, string title, string content, string language);

        PushNotificationSetting UpdateMobileToken(int userId, string mobileToken);

        PushNotificationSetting SwitchNotification(int userId, bool switchFlag);

        PushNotificationSetting GetNotificationState(int userId);

        List<NotificationMessage> GetNotificationList(int userId);

        bool ReadNotification(int userId, int messageId);
    }
}
