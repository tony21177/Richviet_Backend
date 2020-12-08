using Frontend.DB.EF.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Richviet.Services.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Richviet.Services.Services
{
    public class FirebaseMessageService : INotificationService
    {
		//firebase
		public static readonly string FIREBASE_URL = "https://fcm.googleapis.com/fcm/send";
		public static readonly string FIREBASE_KEY_SERVER = "AAAAtrOO6bM:APA91bErt9o3Akx3Sj7MSFCqJZkTf3GPObTcUiDZop_TtI4_0rffCENwZApJIgtmiOo700HjqquPJySCSghwHFMpzI7D0BCiqaboHKKzHKH59UztWLUTm3Sbe5DJ-8ep9yvOGLDo45-V";
		private readonly GeneralContext dbContext;
		private readonly ILogger logger;

		public FirebaseMessageService(ILogger<FirebaseMessageService> logger, GeneralContext dbContext)
		{
			this.logger = logger;
			this.dbContext = dbContext;
		}

		private class PushMessage
		{
			[JsonProperty("notification")]
			public dynamic Notification { get; set; }

			[JsonProperty("to")]
			public string Token { get; set; }

			[JsonProperty("data")]
			public dynamic Data { get; set; }
		}

		private dynamic Push(PushMessage message) {
			try {
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(FIREBASE_URL);
				request.Method = "POST";
				request.Headers.Add("Authorization", "key=" + FIREBASE_KEY_SERVER);
				request.ContentType = "application/json";
				string json = JsonConvert.SerializeObject(message);
				byte[] byteArray = Encoding.UTF8.GetBytes(json);
				request.ContentLength = byteArray.Length;
				Stream dataStream = request.GetRequestStream();
				dataStream.Write(byteArray, 0, byteArray.Length);
				dataStream.Close();
				HttpWebResponse response = (HttpWebResponse)request.GetResponse();
				if (response.StatusCode == HttpStatusCode.Accepted || response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created) {
					StreamReader read = new StreamReader(response.GetResponseStream());
					string result = read.ReadToEnd();
					read.Close();
					response.Close();
					dynamic stuff = JsonConvert.DeserializeObject(result);
					return stuff;
				}
				return null;
			} catch (Exception e) {
				return e.Message;
				//throw new Exception("An error has occurred when try to get server response: " + response.StatusCode);
			}
		}

		public void SendPush(string mobileToken, string title, string body) {
			if (mobileToken != null) Push(new PushMessage {
				Token = mobileToken,
				Notification = new { title, body },
				Data = new { data_title = title, data_content = body }
			});
		}

        public PushNotificationSetting UpdateMobileToken(int userId, string mobileToken)
        {
			try
			{
				PushNotificationSetting setting = dbContext.PushNotificationSetting.SingleOrDefault(x => x.UserId == userId);
				if (setting != null)
				{
					setting.MobileToken = mobileToken;
					dbContext.SaveChanges();
					return setting;
				}
				else
                {
					PushNotificationSetting newSetting = new PushNotificationSetting
					{
						UserId = userId,
						MobileToken = mobileToken
					};
					dbContext.PushNotificationSetting.Add(newSetting);
					dbContext.SaveChanges();
					return newSetting;
				}
			}
			catch (Exception ex)
			{
				logger.LogDebug(ex.Message);
			}
			return null;
        }

        public PushNotificationSetting SwitchNotification(int userId, bool switchFlag)
        {
			try
			{
				PushNotificationSetting setting = dbContext.PushNotificationSetting.SingleOrDefault(x => x.UserId == userId);
				if (setting != null)
				{
					setting.IsTurnOn = switchFlag;
					dbContext.SaveChanges();
					return setting;
				}
				else
				{
					PushNotificationSetting newSetting = new PushNotificationSetting
					{
						UserId = userId,
						IsTurnOn = switchFlag
					};
					dbContext.PushNotificationSetting.Add(newSetting);
					dbContext.SaveChanges();
					return newSetting;
				}
			}
			catch (Exception ex)
			{
				logger.LogDebug(ex.Message);
			}
			return null;
		}

        public PushNotificationSetting GetNotificationState(int userId)
        {
            try
            {
				return dbContext.PushNotificationSetting.Single(x => x.UserId == userId);
			}
			catch (Exception ex)
			{
				logger.LogDebug(ex.Message);
			}
			return null;
		}

        public bool SaveNotificationMessage(int userId, string title, string body, string language)
        {
			try
			{
				NotificationMessage message = new NotificationMessage
				{
					UserId = userId,
					Title = title,
					Content = body,
					Language = language
				};
				dbContext.NotificationMessage.Add(message);
				dbContext.SaveChanges();
				return true;
			}
			catch (Exception ex)
			{
				logger.LogDebug(ex.Message);
			}
			return false;
		}

        public List<NotificationMessage> GetNotificationList(int userId)
        {
			try
			{
				return dbContext.NotificationMessage.Where(x => x.UserId == userId).OrderByDescending(y => y.CreateTime).ToList();
			}
			catch (Exception ex)
			{
				logger.LogDebug(ex.Message);
			}
			return new List<NotificationMessage>();
		}

        public bool ReadNotification(int userId, int messageId)
        {
			try
            {
				NotificationMessage message = dbContext.NotificationMessage.SingleOrDefault(x => x.Id == messageId && x.UserId == userId);
				message.IsRead = true;
				dbContext.SaveChanges();
				return true;
            }
			catch(Exception ex)
            {
				logger.LogDebug(ex.Message);
			}
			return false;
        }
    }
}
