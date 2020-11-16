using Frontend.DB.EF.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Richviet.Services.Contracts;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Richviet.Services.Services
{
    public class FirebaseService : IFirebaseService
    {
		//firebase
		public static readonly string FIREBASE_URL = "https://fcm.googleapis.com/fcm/send";
		public static readonly string FIREBASE_KEY_SERVER = "";
		private readonly GeneralContext dbContext;
		private readonly ILogger logger;

		public FirebaseService(ILogger<FirebaseService> logger, GeneralContext dbContext)
		{
			this.logger = logger;
			this.dbContext = dbContext;
		}

		private class PushMessage
		{
			[JsonProperty("notification")]
			public dynamic Data { get; set; }

			[JsonProperty("to")]
			public string To { get; set; }
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

		public void SendPush(string to, string title, string body) {
			if (to != null) Push(new PushMessage {
				To = to,
				Data = new { title,	body}
			});
		}

        public bool UpdateMobileToken(int userId, string mobileToken)
        {
			try
			{
				PushNotificationSetting setting = dbContext.PushNotificationSetting.SingleOrDefault(x => x.UserId == userId);
				if (setting != null)
				{
					setting.MobileToken = mobileToken;
					dbContext.SaveChanges();
					return true;
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
					return true;
				}
			}
			catch (Exception ex)
			{
				logger.LogDebug(ex.Message);
			}
			return false;
        }

        
    }
}
