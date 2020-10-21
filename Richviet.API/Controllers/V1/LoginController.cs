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
    [Route("api/login")]//required for default versioning
    [Route("api/v{version:apiVersion}/login")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly IUserService userService;
        private readonly JwtHandler _jwtHandler;

        public LoginController(IUserService userService, JwtHandler jwtHandler)
        {
            this.userService = userService;
            this._jwtHandler = jwtHandler;
        }

        [HttpPost]
        public ActionResult<MessageModel<Object>> Login([FromBody] LoginUserRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            LoginType loginType;
            if(!Enum.TryParse(loginRequest.loginType,false,out loginType))
            {
                return BadRequest();
            }

            var loginUserRegistger = new UserRegisterType()
            {
                AuthPlatformId = loginRequest.userId,
                Email = loginRequest.email,
                RegisterType = (byte)loginType,
                Name = loginRequest.name
            };
            bool isVerified = userService.VerifyUserInfo(loginRequest.accessToken, loginUserRegistger).Result;
            if (!isVerified)
                return Unauthorized(new MessageModel<Object>
                {
                    Status = (int)HttpStatusCode.Unauthorized,
                    Success = false,
                    Msg = "Unauthorized",
                    Data = null
                }); ;
            if (userService.GetUser(loginUserRegistger).Result == null)
            {
                userService.AddNewUser(loginUserRegistger).Wait();
            }
            var loginUser = userService.GetUser(loginUserRegistger).Result;

            var accessToken = _jwtHandler.CreateAccessToken(loginUser.Id, loginUser.Email, loginUser.Name, loginRequest.countryOfApp);
            return Ok(new MessageModel<Object>
            {
                Data = new 
                {
                    AccessToken = accessToken
                }
            });
        }
    }
}
