
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Richviet.Services.Constants;
using Richviet.Services.Contracts;
using Richviet.Services.Models;
using System;
using System.Collections;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Richviet.Services
{
    public class FacebookAuthService : IAuthService
    {
        private readonly ILogger logger;
        private readonly IConfiguration configuration;
        private readonly HttpClient httpClient;
        public LoginType LoginType { get; } = LoginType.FB;

        public FacebookAuthService(ILogger<FacebookAuthService> logger, IConfiguration configuration)
        {
            this.configuration = configuration;
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            httpClient = new HttpClient
            {
                BaseAddress = new Uri(this.configuration["Facebook:ApiBaseUrl"])
            };
        }


        public async Task<dynamic> VerifyUserInfo(string accessToken, string permissions, UserRegisterType loginUser)
        {
            //debug token
            var isTokenValid = await VerifyAccessToken(accessToken);
            if (!isTokenValid) return false;

            // verify user info
            dynamic result =  await GetAsync<dynamic>(accessToken, "me", $"fields={permissions}");
            
            if (result.GetValue("error") != null)
            {
                return null;
            }

            if (result == null)
            {
                throw new Exception("User from this token not exist");
            }

            // check fb id is matched 
            var id = result.GetValue("id").ToString();
            if (loginUser.AuthPlatformId.Equals(id))
            {
                return result;
            }

            return null;
        }

        private async Task<bool> VerifyAccessToken(string accessToken)
        {
            var response = await httpClient.GetAsync($"debug_token?input_token={accessToken}&access_token={accessToken}");
            if (!response.IsSuccessStatusCode)
            {
                var errorResult = await response.Content.ReadAsStringAsync();
                logger.LogError(errorResult);
                return false;
            }

            var result = await response.Content.ReadAsStringAsync();
            dynamic resultObj = JsonConvert.DeserializeObject(result);
            string appId = resultObj["data"]["app_id"];
            var allowAppIdArray = configuration.GetSection("Facebook:ApiKey").Get<string[]>();
            if (Array.IndexOf(allowAppIdArray, appId)>-1)
            {
                return true;
            }

            return false;
        }

        private async Task<dynamic> GetAsync<T>(string accessToken, string endpoint, string args = null)
        {
            var response = await httpClient.GetAsync($"{endpoint}?access_token={accessToken}&{args}");
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var errorResult = await response.Content.ReadAsStringAsync();
                logger.LogError(errorResult);
                return JsonConvert.DeserializeObject<T>(errorResult);
            }
            if (!response.IsSuccessStatusCode)
                return default(T);

            var result = await response.Content.ReadAsStringAsync();
            

            return JsonConvert.DeserializeObject<dynamic>(result);
        }
    }
}
