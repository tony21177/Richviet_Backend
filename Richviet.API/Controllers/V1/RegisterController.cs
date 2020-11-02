using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Richviet.API.DataContracts.Dto;
using Richviet.API.DataContracts.Responses;
using Richviet.Services.Contracts;
using Richviet.Services.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections;
using AutoMapper;
using Richviet.API.DataContracts.Requests;

namespace Richviet.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/register")]
    [ApiController]

    public class RegisterController : Controller
    {
        private readonly IUserService userService;
        private IMapper mapper;

        public RegisterController(IUserService userService, IMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;
        }

        /// <summary>
        /// 註冊使用者相關資訊
        /// </summary>
        [HttpPut("Register")]
        public ActionResult<MessageModel<Object>> ModifyOwnUserInfo([FromBody] RegisterRequest registerReq)
        {
            UserInfoDTO userModel = null;

            //解JWT
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userID = identity.FindFirst("id").Value;
                Console.WriteLine(identity.FindFirst("id").Value);

                bool isRegister = userService.ReigsterUserById(int.Parse(userID), registerReq);

                if (isRegister == false)
                {
                    return BadRequest();
                }

                UserInfoView userInfo = userService.GetUserInfoById(int.Parse(userID));
                // 將 user 置換成 ViewModel
                userModel = mapper.Map<UserInfoDTO>(userInfo);
            }

            return Ok(new MessageModel<UserInfoDTO>
            {
                Data = userModel
            });
        }
    }
}
