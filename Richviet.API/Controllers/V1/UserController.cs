
using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Richviet.API.DataContracts.Dto;
using Richviet.API.DataContracts.Requests;
using Richviet.API.DataContracts.Responses;
using Richviet.Services.Constants;
using Richviet.Services.Contracts;
using Richviet.Services.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections;
using AutoMapper;
using Richviet.API.DataContracts.Requests;
using System.Security.Claims;

#pragma warning disable 1591
namespace Richviet.API.Controllers.V1
{

    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/user")]
    [ApiController]
    [Authorize]
    #pragma warning disable    1591
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly IBeneficiarService beneficiarService;
        private readonly IPayeeTypeService payeeTypeService;
        private readonly IMapper mapper;

        public UserController(IUserService userService, IBeneficiarService beneficiarService, IPayeeTypeService payeeTypeService, IMapper mapper)
        {
            this.userService = userService;
            this.beneficiarService = beneficiarService;
            this.payeeTypeService = payeeTypeService;
            this.mapper = mapper;
        }

        /// <summary>
        /// 取得登入的使用者相關資訊
        /// </summary>
        [HttpGet("info")]
        public MessageModel<UserInfoDTO> getOwnUserInfo()
        {
            UserInfoDTO userModel = null;

            //解JWT
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userID = identity.FindFirst("id").Value;
                Console.WriteLine(identity.FindFirst("id").Value);
                UserInfoView userInfo = userService.GetUserInfoById(int.Parse(userID));
                // 將 user 置換成 ViewModel
                userModel = mapper.Map<UserInfoDTO>(userInfo);
            }
            return new MessageModel<UserInfoDTO>
            {
                Data = userModel
            };
        }

        /// <summary>
        /// 新增使用者的常用收款人資訊
        /// </summary>
        [HttpPost("beneficiars")]
        //[AllowAnonymous]
        public MessageModel<UserBeneficiarDTO> AddOwnBeneficiarsInfo([FromBody]OftenBeneficiarRequest oftenBeneficiarRequest)
        {
            var userId = int.Parse(User.FindFirstValue("id"));
            OftenBeneficiar oftenBeneficiar = mapper.Map<OftenBeneficiar>(oftenBeneficiarRequest);
            oftenBeneficiar.UserId = userId;
            var payeeType = payeeTypeService.GetPayeeType((PayeeTypeEnum)oftenBeneficiarRequest.PayeeType);
            oftenBeneficiar.PayeeTypeId = payeeType.Id;
            beneficiarService.AddBeneficiar(oftenBeneficiar);
            UserBeneficiarDTO userBeneficiarDTO = mapper.Map<UserBeneficiarDTO>(oftenBeneficiar);



            return new MessageModel<UserBeneficiarDTO>
            {
                Data = userBeneficiarDTO
            };
        }

        /// <summary>
        /// 修改使用者的常用收款人資訊
        /// </summary>
        [HttpPut("beneficiars/{id}")]
        [AllowAnonymous]
        public MessageModel<UserBeneficiarDTO> ModifyOwnBeneficiarsInfo([FromRoute, SwaggerParameter("id,可從/user/beneficiars取得", Required = true)] int id,[FromBody]OftenBeneficiarRequest oftenBeneficiarRequest)
        {

            return new MessageModel<UserBeneficiarDTO>
            {
                Data =
                    new UserBeneficiarDTO
                    {
                        Id = 1,
                        Name = "爸爸",
                        PayeeAddress = "***************932",
                        PayeeId = "",
                        Note = "爸爸帳號",
                        VietName = "第一銀行",
                        EnName = "First Bank",
                        TwName = "第一銀行",
                        UserId = 5,
                        Type = 0
                    }
            };
        }

        /// <summary>
        /// 刪除使用者的常用收款人資訊
        /// </summary>
        [HttpDelete("beneficiars/{id}")]
        [AllowAnonymous]
        public MessageModel<UserBeneficiarDTO> DeleteOwnBeneficiarsInfo([FromRoute, SwaggerParameter("id,可從/user/beneficiars取得", Required = true)] int id)
        {

            return new MessageModel<UserBeneficiarDTO>() { Data = null};
            
        }

        /// <summary>
        /// 取得使用者的常用收款人資訊
        /// </summary>
        [HttpGet("beneficiars")]
        [AllowAnonymous]
        public MessageModel<UserBeneficiarDTO []> GetOwnBeneficiarsInfo()
        {
            
            return new MessageModel<UserBeneficiarDTO []>
            {
                Data = new UserBeneficiarDTO[2]
                {
                    new UserBeneficiarDTO
                    {
                        Id = 1,
                        Name = "爸爸",
                        PayeeAddress = "***************932",
                        PayeeId = "",
                        Note = "爸爸帳號",
                        VietName = "第一銀行",
                        EnName = "First Bank",
                        TwName = "第一銀行",
                        UserId = 5,
                        Type = 0
                    },
                    new UserBeneficiarDTO
                    {
                        Id = 1,
                        Name = "媽",
                        PayeeAddress = "***************552",
                        PayeeId = "",
                        Note = "媽的帳號",
                        VietName = "國泰銀行",
                        EnName = "Guo Tai Bank",
                        TwName = "國泰銀行",
                        UserId = 5,
                        Type = 0
                    }
                }
            };
        }


    }
}
