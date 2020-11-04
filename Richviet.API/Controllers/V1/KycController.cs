using System;
using Microsoft.AspNetCore.Mvc;
using Richviet.API.DataContracts.Dto;
using Richviet.API.DataContracts.Responses;
using Richviet.Services.Contracts;
using Richviet.Services.Models;
using AutoMapper;
using Richviet.API.DataContracts.Requests;
using Microsoft.AspNetCore.Authorization;

namespace Richviet.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/kyc")]
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
        /// 更改KYC審核狀態
        /// </summary>
        [HttpPut("kycStatus")]
        [AllowAnonymous]

        public ActionResult<MessageModel<Object>> ModifyKycStatus([FromBody] KycRequest kycReq)
        {

            bool isChangeKycStatus = userService.ChangeKycStatusById(kycReq);

            if (isChangeKycStatus == false)
            {
                return BadRequest();
            }

            UserInfoView userInfo = userService.GetUserInfoById(kycReq.Id);
            UserInfoDTO userModel = mapper.Map<UserInfoDTO>(userInfo);

            return Ok(new MessageModel<Object>
            {
                Data = new
                {
                    UserId = userModel.Id,
                    UserName = userModel.ArcName,
                    KYCStatus = userModel.KycStatus
                }
            });
        }
    }
}
