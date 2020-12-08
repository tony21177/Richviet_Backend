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
        private readonly IFirebaseService firebaseService;

        public NotificationController(IMapper mapper, ILogger<NotificationController> logger, IFirebaseService firebaseService)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.firebaseService = firebaseService;
        }

        /// <summary>
        /// test push
        /// </summary>
        [HttpPost("testpush")]
        [AllowAnonymous]
        public MessageModel<bool> TestPush()
        {
            firebaseService.SendPush("fEXQFybSwzE:APA91bHpFgfrZ1A2URzz7zkxwAIFO99ABDHlcBQaBykLMRpB0voy4LXfycMpaWQ-co0GC5YOpVhgHY5wBfgMw-eStjhCApWpbm7oMJ8yrnJinshuI2-jHtInwXIjSdTdd_KYcfUOxlg9", "test", "20201105");
            return new MessageModel<bool>
            {
                Data = true
            };
        }
    }
}
