using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Richviet.Admin.API.DataContracts.Dto;
using Richviet.Admin.API.DataContracts.Requests;
using Richviet.Admin.API.DataContracts.Responses;
using Richviet.Services.Contracts;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Richviet.Admin.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("admin/v{version:apiVersion}/useradmin")]
    [ApiController]
    public class UserAdminController : ControllerBase
    {
        private readonly ILogger Logger;
        private readonly IMapper mapper;
        private readonly IUserAdminService userAdminService;

        public UserAdminController(ILogger<UserAdminController> logger, 
            IMapper mapper, 
            IUserAdminService userAdminService)
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.mapper = mapper;
            this.userAdminService = userAdminService;
        }

        /// <summary>
        /// 取得使用者列表
        /// </summary>
        [HttpGet]
        public MessageModel<List<UserAdminListDTO>> GetUserList()
        {
            List<UserAdminListDTO> userList = userAdminService.GetUserList();
            return new MessageModel<List<UserAdminListDTO>>
            {
                Data = userList
            };
        }

        /// <summary>
        /// 取得使用者過濾列表
        /// </summary>
        [HttpGet("filter")]
        public MessageModel<List<UserAdminListDTO>> GetUserFilterList(UserFilterListRequest request)
        {
            List<UserAdminListDTO> userList = userAdminService.GetUserFilterList(request);
            return new MessageModel<List<UserAdminListDTO>>
            {
                Data = userList
            };
        }

        /// <summary>
        /// 取得使用者詳細資料
        /// </summary>
        [HttpGet("{id}")]
        public MessageModel<UserDetailDTO> GetUserDetail([FromRoute, SwaggerParameter("id,可從/useradmin取得", Required = true)] int id)
        {
            UserDetailDTO userDetail = userAdminService.GetUserDetail(id);
            return new MessageModel<UserDetailDTO>
            {
                Data = userDetail
            };
        }



    }


}
