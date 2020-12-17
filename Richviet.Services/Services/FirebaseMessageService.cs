using Frontend.DB.EF.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Richviet.Services.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Richviet.Services.Services
{
    public class FirebaseMessageService : INotificationService
    {
		//firebase
		public static readonly string FIREBASE_URL = "https://fcm.googleapis.com/fcm/send";
		public static readonly string FIREBASE_SERVER_KEY = "AAAAtrOO6bM:APA91bErt9o3Akx3Sj7MSFCqJZkTf3GPObTcUiDZop_TtI4_0rffCENwZApJIgtmiOo700HjqquPJySCSghwHFMpzI7D0BCiqaboHKKzHKH59UztWLUTm3Sbe5DJ-8ep9yvOGLDo45-V";
		private readonly GeneralContext dbContext;
		private readonly ILogger logger;
		private readonly HttpClient httpClient;

		public FirebaseMessageService(ILogger<FirebaseMessageService> logger, GeneralContext dbContext)
		{
			this.logger = logger;
			this.dbContext = dbContext;
			httpClient = new HttpClient();
		}

		private string GetTokenByUserId(int userId)
		{
			try
			{
				PushNotificationSetting setting = dbContext.PushNotificationSetting.SingleOrDefault(x => x.UserId == userId);
				if (setting != null)
				{
					return setting.MobileToken;
				}
			}
			catch (Exception ex)
			{
				logger.LogError(ex.Message);
			}
			return null;
		}

		private async Task<bool> PostFirebaseApi(PushMessage message)
		{
			try
			{
				httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("key", "=" + FIREBASE_SERVER_KEY);
				httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				StringContent jsonStr = new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json");
				HttpResponseMessage response = await httpClient.PostAsync(FIREBASE_URL, jsonStr);				
				string result = await response.Content.ReadAsStringAsync();
				if(response.IsSuccessStatusCode)
                {
					return true;
                }
				else
                {
					logger.LogError(result);
                }
			}
			catch (Exception ex)
			{
				logger.LogError(ex.Message);
			}
			return false;
		}

		public async Task<bool> SendNotification(int userId, string title, string body)
		{
			try
			{
				string mobileToken = GetTokenByUserId(userId);
				if (mobileToken != null)
				{
					string title_loc_key = "TEST_TITLE_LOC_KEY";
					string body_loc_key = "TEST_BODY_LOC_KEY";
					return await PostFirebaseApi(new PushMessage
					{
						Token = mobileToken,
						Notification = new NotificationData
						{ 
							Title = title,
							Body = body,
							TitleLocKey = title_loc_key,
							BodyLocKey = body_loc_key
						}
						//Data = new { data_title = title, data_content = body, data_key = "TEST_FOR_IOS"}
					});
				}
				return false;
			}
			catch (Exception ex)
			{
				logger.LogError(ex.Message);
			}
			return false;
		}

		public async Task<bool> SendNotification(int userId, string title, string body, string titleLocKey, string bodyLocKey)
		{
			try
			{
				string mobileToken = GetTokenByUserId(userId);
				if (mobileToken != null)
				{
					return await PostFirebaseApi(new PushMessage
					{
						Token = mobileToken,
						Notification = new NotificationData
						{
							Title = title,
							Body = body,
							TitleLocKey = titleLocKey,
							BodyLocKey = bodyLocKey
						}
					});
				}
				return false;
			}
			catch (Exception ex)
			{
				logger.LogError(ex.Message);
			}
			return false;
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

        public bool SaveNotificationMessage(int userId, string title, string content, string language)
        {
			try
			{
				NotificationMessage message = new NotificationMessage
				{
					UserId = userId,
					Title = title,
					Content = content,
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

		[Obsolete]
		public async Task<bool> SaveAndSendNotification(int userId, string title, string body, string language)
        {
			try
			{
				NotificationMessage message = new NotificationMessage
				{
					UserId = userId,
					Title = title,
					Content = body,
					Language = language,
					IsRead = true
				};
				dbContext.NotificationMessage.Add(message);
				dbContext.SaveChanges();
				return await SendNotification(userId, title, body);
			}
			catch (Exception ex)
			{
				logger.LogDebug(ex.Message);
			}
			return false;
		}

		public async Task<bool> SaveAndSendNotification(int userId, string title, string body, string titleLocKey, string bodyLocKey)
		{
			try
			{
				NotificationMessage message = new NotificationMessage
				{
					UserId = userId,
					Title = title,
					Content = body,
					Language = "en-US",
					IsRead = true
				};
				dbContext.NotificationMessage.Add(message);
				dbContext.SaveChanges();
				return await SendNotification(userId, title, body, titleLocKey, bodyLocKey);
			}
			catch (Exception ex)
			{
				logger.LogDebug(ex.Message);
			}
			return false;
		}

		private class PushMessage
		{
			[JsonProperty("notification")]
			public NotificationData Notification { get; set; }

			[JsonProperty("to")]
			public string Token { get; set; }

			/*[JsonProperty("data")]
			public dynamic Data { get; set; }*/
		}

		private class NotificationData
        {
			[JsonProperty("title")]
			public string Title { get; set; }

			[JsonProperty("body")]
			public string Body { get; set; }

			[JsonProperty("title_loc_key")]
			public string TitleLocKey { get; set; }

			[JsonProperty("body_loc_key")]
			public string BodyLocKey { get; set; }
		}
	}
}
