using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Richviet.Admin.API.DataContracts.Requests;
using Richviet.Admin.API.DataContracts.Responses;
using Users.Domains.Users.Command.UseCase;
using AutoMapper;
using Users.Domains.Users.Command.Request;

namespace Richviet.Admin.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("user/v{version:apiVersion}/command")]
    [ApiController]
    public class UserCommandController : ControllerBase
    {
        private readonly UserModifier modifier;
        private readonly IMapper mapper;

        public UserCommandController(UserModifier modifier)
        {
            this.modifier = modifier;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserModifyRequest, UserVoRequest>();
                cfg.CreateMap<UserLevelModifyRequest, UserLevelModifyVoRequest>();
            });

            this.mapper = configuration.CreateMapper();
        }

        [HttpPost("modify")]
        public MessageModel<string> ModifyUser(UserModifyRequest request)
        {
            var userVoRequest =  this.mapper.Map<UserVoRequest>(request);
            this.modifier.Modify(userVoRequest);

            return new MessageModel<string>()
            {
                Data = "modify success"
            };
        }

        [HttpPost("modifyLevel")]
        public MessageModel<string> ModifyLevel(UserLevelModifyRequest request)
        {
            var levelVoRequest = this.mapper.Map<UserLevelModifyVoRequest>(request);

            this.modifier.ModifyUserLevel(levelVoRequest);
            return new MessageModel<string>()
            {
                Data = "modify success"
            };
        }
    }

}
