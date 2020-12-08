using AutoMapper;
using Frontend.DB.EF.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Richviet.API.DataContracts.Dto;
using Richviet.API.DataContracts.Requests;
using Richviet.API.DataContracts.Responses;
using Richviet.Services.Contracts;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Richviet.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/notification")]
    [ApiController]
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly IMapper mapper;
        private readonly ILogger logger;
        private readonly INotificationService notificationService;

        public NotificationController(IMapper mapper, ILogger<NotificationController> logger, INotificationService firebaseService)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.notificationService = firebaseService;
        }

        /// <summary>
        /// 更新使用者MobileToken
        /// </summary>
        [HttpPost("mobiletoken")]
        //[AllowAnonymous]
        public MessageModel<NotificationSettingDTO> UpdateMobileToken([FromBody] NotificationTokenRequest request)
        {
            var userId = int.Parse(User.FindFirstValue("id"));
            //int userId = 1;
            PushNotificationSetting result = notificationService.UpdateMobileToken(userId, request.MobileToken);
            NotificationSettingDTO dto = mapper.Map<NotificationSettingDTO>(result);
            return new MessageModel<NotificationSettingDTO>
            {
                Msg = result==null?"error":"",
                Data = dto
            };
        }

        /// <summary>
        /// 開關使用者通知
        /// </summary>
        [HttpPost("switch")]
        //[AllowAnonymous]
        public MessageModel<NotificationSettingDTO> SwitchNotification([FromBody] NotificationSettingRequest request)
        {
            var userId = int.Parse(User.FindFirstValue("id"));
            PushNotificationSetting result = notificationService.SwitchNotification(userId, request.IsTurnOn);
            NotificationSettingDTO dto = mapper.Map<NotificationSettingDTO>(result);
            return new MessageModel<NotificationSettingDTO>
            {
                Msg = result == null ? "error" : "",
                Data = dto
            };
        }

        /// <summary>
        /// 取得使用者通知狀態
        /// </summary>
        [HttpGet("state")]
        //[AllowAnonymous]
        public MessageModel<NotificationSettingDTO> GetNotificationState()
        {
            var userId = int.Parse(User.FindFirstValue("id"));
            PushNotificationSetting result = notificationService.GetNotificationState(userId);
            NotificationSettingDTO dto = mapper.Map<NotificationSettingDTO>(result);
            return new MessageModel<NotificationSettingDTO>
            {
                Msg = result == null ? "error" : "",
                Data = dto
            };
        }

        /// <summary>
        /// 修改使用者通知訊息為已讀
        /// </summary>
        [HttpPost("read/{id}")]
        //[AllowAnonymous]
        public MessageModel<bool> ReadNotification([FromRoute, SwaggerParameter("通知訊息的id", Required = true)] int id)
        {
            var userId = int.Parse(User.FindFirstValue("id"));
            //var userId = 1;
            bool result = notificationService.ReadNotification(userId, id);
            return new MessageModel<bool>
            {
                Msg = result ? "success" : "error",
                Success = result,
                Data = result
            };
        }

        /// <summary>
        /// 取得使用者通知訊息列表
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public MessageModel<List<NotificationMessageDTO>> GetNotificationList()
        {
            var userId = int.Parse(User.FindFirstValue("id"));
            //var userId = 1;
            List<NotificationMessage> messageList = notificationService.GetNotificationList(userId);
            List<NotificationMessageDTO> dto = mapper.Map<List<NotificationMessageDTO>>(messageList);
            return new MessageModel<List<NotificationMessageDTO>>
            {
                Data = dto
            };
        }
    }
}
