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
using Richviet.Admin.API.DataContracts.Requests;

namespace Richviet.Admin.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("admin/v{version:apiVersion}/kyc")]
    [ApiController]
    //[Authorize(Roles = "adminManager")]
    public class KycController : Controller
    {
        private readonly IUserService userService;
        private IMapper mapper;
        private readonly INotificationService notificationService;

        public KycController(IUserService userService, IMapper mapper, INotificationService firebaseService)
        {
            this.userService = userService;
            this.mapper = mapper;
            this.notificationService = firebaseService;
        }

        /// <summary>
        /// 更改使用者kyc狀態
        /// </summary>
        [HttpPut("{userId}")]

        public ActionResult<MessageModel<Object>> ChangeUserKyc([FromBody] KycRequest kycRequest,[FromRoute, SwaggerParameter("使用者ID", Required = true)] long userId)
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

            var result = new MessageModel<Object>
            {
                Status = (int)HttpStatusCode.BadRequest,
                Success = false,
                Msg = "Fail to Operate"
            };

            if (userService.ChangeKycStatusByUserId((KycStatusEnum)kycRequest.KycStatus, userId))
            {
                if((KycStatusEnum)kycRequest.KycStatus == KycStatusEnum.PASSED_KYC_FORMAL_MEMBER)
                {
                    notificationService.SaveAndSendNotification((int)userId, "Successful Registration", "Your registration have been confirmed", "en-US");
                }
                if ((KycStatusEnum)kycRequest.KycStatus == KycStatusEnum.FAILED_KYC)
                {
                    notificationService.SaveAndSendNotification((int)userId, "Unsuccessful Registration", "You do not pass The KYC procedure", "en-US");
                }


                result.Status = (int)HttpStatusCode.OK;
                result.Success = true;
                result.Msg = "Successful Operation";
                return Ok(result);
                
            }

            return BadRequest(result);
        }
    }
}
