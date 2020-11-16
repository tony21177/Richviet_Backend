using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Richviet.API.DataContracts.Dto;
using Richviet.API.DataContracts.Responses;
using Richviet.Services.Contracts;
using Frontend.DB.EF.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections;
using AutoMapper;
using Richviet.API.DataContracts.Requests;
using Richviet.Tools.Utility;
using Microsoft.Extensions.Logging;
using Richviet.Services.Constants;
using System.Net;
using Richviet.BackgroudTask.Arc;
using System.Threading.Tasks;
using Hangfire;

namespace Richviet.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/register")]
    [ApiController]
    [Authorize]
    public class RegisterController : Controller
    {
        private readonly IUserService userService;
        private readonly JwtHandler jwtHandler;
        private readonly IMapper mapper;
        private readonly ArcValidationTask arcValidationTask;

        public RegisterController(IUserService userService, JwtHandler jwtHandler, IMapper mapper, ArcValidationTask arcValidationTask)
        {
            this.userService = userService;
            this.jwtHandler = jwtHandler;
            this.mapper = mapper;
            this.arcValidationTask = arcValidationTask;
        }

        /// <summary>
        /// 註冊使用者相關資訊
        /// </summary>
        [HttpPut("register")]
        public ActionResult<MessageModel<RegisterResponseDTO>> ModifyOwnUserInfo([FromBody] RegisterRequest registerReq)
        {
            UserInfoDTO userModel = null;
            Tools.Utility.TokenResource accessToken = null;

            
            var userId = int.Parse(User.FindFirstValue("id"));
            UserArc userArc = userService.GetUserArcById(userId);
            if (userArc.KycStatus != (byte)KycStatusEnum.DRAFT_MEMBER)
            {
                return BadRequest(new MessageModel<RegisterResponseDTO>
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Success = false,
                    Msg = "Only Draft member can register"
                }
                );
            }
            if (String.IsNullOrEmpty(userArc.IdImageA) || String.IsNullOrEmpty(userArc.IdImageB))
            {
                return BadRequest(new MessageModel<RegisterResponseDTO>
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Success = false,
                    Msg = "Please upload your ID photo"
                }
                    );
            }
            User user = userService.GetUserById(userId);
            UserRegisterType userRegisterType = userService.GetUserRegisterTypeById(userId);
            //user data
            user.Phone = registerReq.phone;
            user.Email = userRegisterType.Email;
            user.Gender = (byte)registerReq.gender;
            user.Birthday = registerReq.birthday;

            //userArc data
            userArc.ArcName = registerReq.name;
            userArc.Country = registerReq.country;
            userArc.ArcNo = registerReq.personalID;
            userArc.PassportId = registerReq.passportNumber;
            userArc.BackSequence = registerReq.backCode;
            userArc.ArcIssueDate = registerReq.issue;
            userArc.ArcExpireDate = registerReq.expiry;
            userArc.KycStatus = 1;
            userArc.KycStatusUpdateTime = DateTime.Now;

            //update UserRegisterType data
            userRegisterType.RegisterTime = DateTime.Now;
            


            bool isRegister = userService.ReigsterUser(user,userArc, userRegisterType);

            if (isRegister == false)
            {
                return BadRequest();
            }

            UserInfoView userInfo = userService.GetUserInfoById(userId);
            //// 將 user 置換成 ViewModel
            userModel = mapper.Map<UserInfoDTO>(userInfo);

            accessToken = jwtHandler.CreateAccessToken(userModel.Id, userModel.Email, userModel.ArcName);

            // 系統掃ARC No.
            BackgroundJob.Enqueue(() => userService.SystemVerifyArc(int.Parse(User.FindFirstValue("id"))));

            //return Ok(new MessageModel<UserInfoDTO>
            //{
            //    Data = userModel
            //});

            return Ok(new MessageModel<RegisterResponseDTO>
            {
                Data = new RegisterResponseDTO
                {
                    Jwt = accessToken.Token,
                    kycStatus = (byte)userModel.KycStatus
                }
            });
        }

        
        
    } 
}
