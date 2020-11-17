using System;
using Microsoft.AspNetCore.Mvc;
using Richviet.Services.Contracts;
using Frontend.DB.EF.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Richviet.Admin.API.DataContracts.Responses;
using Swashbuckle.AspNetCore.Annotations;
using Richviet.Services.Constants;
using System.Net;

namespace Richviet.Admin.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("admin/v{version:apiVersion}/kyc")]
    [ApiController]

    public class KycController : Controller
    {
        private readonly IUserService userService;
        private IMapper mapper;

        public KycController(IUserService userService, IMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;
        }

        /// <summary>
        /// 審核通過會員的註冊KYC
        /// </summary>
        [HttpGet("{userId}/pass")]
        [AllowAnonymous]

        public ActionResult<MessageModel<Object>> PassUserKyc([FromRoute, SwaggerParameter("使用者ID", Required = true)] int userId)
        {

            UserArc userArc = userService.GetUserArcById(userId);
            if (userArc == null)
            {
                return BadRequest(new MessageModel<Object>
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Success = false,
                    Msg = "User does not exist"
                }); ;
            }

            if (userArc.KycStatus != (short)KycStatusEnum.ARC_PASS_VERIFY)
            {
                return BadRequest(new MessageModel<Object>
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Success = false,
                    Msg = "Invalid Operation"
                });

            }
            var result = new MessageModel<Object>
            {
                Status = (int)HttpStatusCode.BadRequest,
                Success = false,
                Msg = "Fail to Operate"
            };

            if (userService.ChangeKycStatusByUserId(KycStatusEnum.PASSED_KYC_FORMAL_MEMBER, userId))
            {
                result.Status = (int)HttpStatusCode.OK;
                result.Success = true;
                result.Msg = "Successful Operation";
                return Ok(result);
                
            }

            return BadRequest(result);
        }
    }
}
