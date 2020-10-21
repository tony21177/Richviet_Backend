using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Richviet.Services.Constants;
using Richviet.Services.Contracts;
using Richviet.Services.Models;
using Richviet.Tools.Utility;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Richviet.Services
{
    public class FacebookAuthService : IAuthService
    {
        private readonly ILogger Logger;
        
        public LoginType LoginType { get; } = LoginType.FB;
        private readonly HttpClient _httpClient;

        public FacebookAuthService(ILogger<FacebookAuthService> logger)
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://graph.facebook.com/v8.0/")
            };
            _httpClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        public async Task<bool> VerifyUserInfo(string accessToken,UserRegisterType loginUser)
        {
            dynamic result =  await GetAsync<dynamic>(accessToken, "me", "fields=name,email,picture,birthday,gender");
            
            if (result.GetValue("error") != null)
            {
                string error = Convert.ToString(result.error);
                Logger.LogInformation(error, null);
                string errorMessage = Convert.ToString(result.error.message);
                Logger.LogInformation(errorMessage, null);
                return false;
            }

            if (result == null)
            {
                throw new Exception("User from this token not exist");
            }

            // check fb id is matched 
            var id = result.GetValue("id").ToString();
            if (loginUser.AuthPlatformId.Equals(id))
            {
                return true;
            }

            return false;
        }

        private async Task<dynamic> GetAsync<T>(string accessToken, string endpoint, string args = null)
        {
            var response = await _httpClient.GetAsync($"{endpoint}?access_token={accessToken}&{args}");
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var errorResult = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(errorResult);
            }
            if (!response.IsSuccessStatusCode)
                return default(T);

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<dynamic>(result);
        }
    }
}
