using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Richviet.Admin.API.DataContracts.Responses;
using Microsoft.AspNetCore.Authorization;

namespace Richviet.Admin.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("server/v{version:apiVersion}/health")]
    [ApiController]
    public class HealthController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public MessageModel<string> Check()
        {
           
            return new MessageModel<string>()
            {
                Data = "health"
            };
        }
    }
}
