using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Richviet.API.DataContracts.Requests;
using Richviet.API.DataContracts.Responses;
using Richviet.Services.Constants;
using Richviet.Services.Contracts;
using Richviet.Services.Models;
using Richviet.Tools.Utility;
using System;
using System.Net;

namespace Richviet.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/login")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly IUserService userService;
        private readonly JwtHandler jwtHandler;

        public LoginController(IUserService userService, JwtHandler jwtHandler)
        {
            this.userService = userService;
            this.jwtHandler = jwtHandler;
        }

        /// <summary>
        /// 第三方oauth登入後,此API提供我們自己的accessToken做為authetication
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult<MessageModel<Object>> Login([FromBody] LoginUserRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var loginUserRegistger = new UserRegisterType()
            {
                AuthPlatformId = loginRequest.userId,
                RegisterType = (byte)loginRequest.loginType
            };

            dynamic verifiedData = userService.VerifyUserInfo(loginRequest.accessToken, loginRequest.permissions, loginUserRegistger).Result;

            if (verifiedData == null )
                return Unauthorized(new MessageModel<Object>
                {
                    Status = (int)HttpStatusCode.Unauthorized,
                    Success = false,
                    Msg = "Unauthorized",
                    Data = null
                }); ;

            loginUserRegistger.Name = verifiedData["name"] == null ? "" : verifiedData["name"].ToString();
            loginUserRegistger.Email = verifiedData["email"] ==null?"":verifiedData["email"].ToString();

            Console.WriteLine($"aaaaa:{loginUserRegistger.Email}");

            if (userService.GetUserInfo(loginUserRegistger).Result == null)
            {
                userService.AddNewUserInfo(loginUserRegistger).Wait();
            }
            var loginUser = userService.GetUserInfo(loginUserRegistger).Result;

            var accessToken = this.jwtHandler.CreateAccessToken(loginUser.Id, loginUser.Email, loginUser.Name);
            return Ok(new MessageModel<Object>
            {
                Data = new 
                {
                    AccessToken = accessToken.Token,
                    loginUser.Status,
                    KYCStatus = loginUser.KycStatus
                }
            });
        }
    }
}
