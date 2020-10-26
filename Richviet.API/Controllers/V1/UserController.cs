using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Richviet.API.DataContracts;
using Richviet.API.DataContracts.Responses;
using Richviet.Services.Contracts;
using Richviet.Services.Models;

namespace Richviet.API.Controllers.V1
{

    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/user")]
    [ApiController]
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        /// 取得登入的使用者相關資訊
        /// </summary>
        [HttpGet("info")]
        public MessageModel<UserInfoDTO> getOwnUserInfo()
        {
            //int id = User.Claims
            //UserInfoView userInfo = userService.GetUserById();
            return new MessageModel<UserInfoDTO>
            {
                Data = new UserInfoDTO()
            };
        }

    }
}
