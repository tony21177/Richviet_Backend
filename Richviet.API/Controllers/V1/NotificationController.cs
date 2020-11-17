using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Richviet.API.DataContracts.Requests;
using Richviet.API.DataContracts.Responses;
using Richviet.Services.Contracts;
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
        private readonly IFirebaseService firebaseService;

        public NotificationController(IMapper mapper, ILogger<NotificationController> logger, IFirebaseService firebaseService)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.firebaseService = firebaseService;
        }

        /// <summary>
        /// 更新使用者MobileToken
        /// </summary>
        [HttpPost("mobiletoken")]
        //[AllowAnonymous]
        public MessageModel<bool> UpdateMobileToken([FromBody] NotificationSettingRequest request)
        {
            var userId = int.Parse(User.FindFirstValue("id"));
            bool result = firebaseService.UpdateMobileToken(userId, request.MobileToken);
            return new MessageModel<bool>
            {
                Data = result
            };
        }

        /// <summary>
        /// 開關使用者通知
        /// </summary>
        [HttpPost("switch")]
        //[AllowAnonymous]
        public MessageModel<bool> SwitchNotification([FromBody] NotificationSettingRequest request)
        {
            var userId = int.Parse(User.FindFirstValue("id"));
            //int userId = 1;
            bool result = firebaseService.SwitchNotification(userId, request.IsTurnOn);
            return new MessageModel<bool>
            {
                Data = result
            };
        }
    }
}
