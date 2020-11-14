using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Richviet.Admin.API.DataContracts.Requests;
using Richviet.Admin.API.DataContracts.Responses;
using Richviet.Services.Users.Command.UseCase;

namespace Richviet.Admin.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("user/v{version:apiVersion}/command")]
    [ApiController]
    public class UserCommandController : Controller
    {
        private readonly UserModifier modifier;

        public UserCommandController(UserModifier modifier)
        {
            this.modifier = modifier;
        }

        [HttpPost("modify")]
        public MessageModel<string> ModifyUser(UserModifyRequest request)
        {
            this.modifier.Modify(request);

            return new MessageModel<string>()
            {
                Data = "modify success"
            };
        }
    }
}
