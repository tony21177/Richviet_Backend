using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Richviet.Admin.API.DataContracts.Responses;
using Richviet.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Richviet.Admin.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("admin/v{version:apiVersion}/notification")]
    [ApiController]
    
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
        /// test push
        /// </summary>
        [HttpPost("testpush")]
        [AllowAnonymous]
        public async Task<MessageModel<bool>> TestPush()

        {
            await notificationService.SendNotification(1, "title " + DateTime.UtcNow, "content " + DateTime.UtcNow);
            return new MessageModel<bool>
            {
                Data = true
            };
        }

        /// <summary>
        /// test push
        /// </summary>
        [HttpPost("testsave")]
        public MessageModel<bool> SaveMessage()
        {
            bool result = notificationService.SaveNotificationMessage(1, "title " + DateTime.UtcNow, "content " + DateTime.UtcNow, "zh-TW");
            return new MessageModel<bool>
            {
                Data = result
            };
        }

        /// <summary>
        /// test push
        /// </summary>
        [HttpPost("testsavepush")]
        public async Task<MessageModel<bool>> SaveAndPush()
        {
            bool result = await notificationService.SaveAndSendNotification(1, "title " + DateTime.UtcNow, "content " + DateTime.UtcNow, "TEST_TITLE_LOC_KEY", "TEST_BODY_LOC_KEY");
            return new MessageModel<bool>
            {
                Data = result
            };
        }
    }
}
