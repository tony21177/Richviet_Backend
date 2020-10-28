using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;


namespace Richviet.API.DataContracts.Requests
{
    [SwaggerSchema(Required = new[] { "Description" })]
    public class LoginUserRequest
    {
        [Required]
        [SwaggerSchema("oauth2登入串接平台的id")]
        public string userId { get; set; }
        [Required]
        [SwaggerSchema("oauth2登入串接平台取得的accessToken")]
        public string accessToken { get; set; }
        [Required]
        [SwaggerSchema("前端SDK所要求的權限字串,後端依照此字串去跟平台要資料")]
        public string permissions { get; set; }
        [Required]
        [SwaggerSchema("登入方式)0:我們平台本身,1:FB")]
        public int loginType { get; set; }
        //public string countryOfApp { get; set; }
        //public TokenResource refreshToken { get; set; }
    }
}
