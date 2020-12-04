using AutoMapper;
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
        public MessageModel<bool> TestPush()
        {
            firebaseService.SendPush("fEXQFybSwzE:APA91bFyMZWDnUTHrIFKrwrLD2SaFqA71fc-yIRZBQnFxy7ZQDeLNnlJNAeru4jetMZtpnZw_pvjQDG4f3xJk1FmJxs4GtvnYSB5_WQiymi9vXW4M9RHWludNKImxTyZPiXlf2ChHWmD", "test", "20201204");
            return new MessageModel<bool>
            {
                Data = true
            };
        }
    }
}
