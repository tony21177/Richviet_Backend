using Frontend.DB.EF.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Richviet.Services.Contracts
{
    public interface INotificationService
    {
        [Obsolete]
        Task<bool> SendNotification(int userId, string title, string body);

        Task<bool> SendNotification(int userId, string title, string body, string titleLocKey, string bodyLocKey);

        bool SaveNotificationMessage(int userId, string title, string content, string language);

        [Obsolete]
        Task<bool> SaveAndSendNotification(int userId, string title, string body, string language);

        Task<bool> SaveAndSendNotification(int userId, string title, string body, string titleLocKey, string bodyLocKey);

        PushNotificationSetting UpdateMobileToken(int userId, string mobileToken);

        PushNotificationSetting SwitchNotification(int userId, bool switchFlag);

        PushNotificationSetting GetNotificationState(int userId);

        List<NotificationMessage> GetNotificationList(int userId);

        bool ReadNotification(int userId, int messageId);
    }
}
