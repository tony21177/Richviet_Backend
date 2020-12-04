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
using Swashbuckle.AspNetCore.Annotations;

namespace Richviet.Admin.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("admin/user/v{version:apiVersion}/command")]
    [ApiController]
    public class UserCommandController : ControllerBase
    {
        private readonly UserModifier modifier;
        private readonly UserRemoverForDevUse userRemoverForDev;
        private readonly IMapper mapper;

        public UserCommandController(UserModifier modifier, UserRemoverForDevUse userRemoverForDev)
        {
            this.modifier = modifier;
            this.userRemoverForDev = userRemoverForDev;

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


        [HttpDelete("{userId}")]
        public MessageModel<string> DeleteUser([FromRoute, SwaggerParameter("會員id", Required = true)] long userId)
        {
            userRemoverForDev.removeUser(userId);

            return new MessageModel<string>()
            {
     
                Msg = "delete success"
            };
        }
    }

}
