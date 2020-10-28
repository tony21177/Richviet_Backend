
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Richviet.Services.Constants;
using Richviet.Services.Contracts;
using Richviet.Services.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Richviet.Services
{
    public class FacebookAuthService : IAuthService
    {
        private readonly ILogger Logger;

        private readonly IConfiguration _configuration;

        public LoginType LoginType { get; } = LoginType.FB;
        private readonly HttpClient _httpClient;

        public FacebookAuthService(ILogger<FacebookAuthService> logger, IConfiguration configuration)
        {
            this._configuration = configuration;
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_configuration["Facebook:ApiBaseUrl"])
            };
            _httpClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        public async Task<bool> VerifyUserInfo(string accessToken, string permissions, UserRegisterType loginUser)
        {
            //debug token
            var isTokenValid = await VerifyAccessToken(accessToken);
            if (!isTokenValid) return false;

            // verify user info
            dynamic result =  await GetAsync<dynamic>(accessToken, "me", $"fields={permissions}");
            
            if (result.GetValue("error") != null)
            {
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

        private async Task<bool> VerifyAccessToken(string accessToken)
        {
            var response = await _httpClient.GetAsync($"debug_token?input_token={accessToken}&access_token={accessToken}");
            if (!response.IsSuccessStatusCode)
            {
                var errorResult = await response.Content.ReadAsStringAsync();
                Logger.LogError(errorResult);
                return false;
            }

            var result = await response.Content.ReadAsStringAsync();
            dynamic resultObj = JsonConvert.DeserializeObject(result);
            string appId = resultObj["data"]["app_id"];
            var allowAppIdArray = _configuration.GetSection("Facebook:ApiKey").Get<string[]>();
            if (Array.IndexOf(allowAppIdArray, appId)>-1)
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
                Logger.LogError(errorResult);
                return JsonConvert.DeserializeObject<T>(errorResult);
            }
            if (!response.IsSuccessStatusCode)
                return default(T);

            var result = await response.Content.ReadAsStringAsync();
            

            return JsonConvert.DeserializeObject<dynamic>(result);
        }
    }
}
