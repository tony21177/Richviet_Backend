
using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Richviet.API.DataContracts.Dto;
using Richviet.API.DataContracts.Requests;
using Richviet.API.DataContracts.Responses;
using Richviet.Services.Constants;
using Richviet.Services.Contracts;
using Frontend.DB.EF.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Collections;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Net;

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
        private readonly ILogger logger;

        public UserController(IUserService userService, IBeneficiarService beneficiarService, IPayeeTypeService payeeTypeService, IMapper mapper, ILogger<UserController> logger)
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
        public ActionResult<MessageModel<UserInfoDTO>> getOwnUserInfo()
        {
            UserInfoDTO userModel = null;

            //解JWT
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userID = identity.FindFirst("id").Value;
                Console.WriteLine(identity.FindFirst("id").Value);
                UserInfoView userInfo = userService.GetUserInfoById(int.Parse(userID));
                if(userInfo == null)
                {
                    return BadRequest(new MessageModel<UserInfoDTO>
                    {
                        Status = (int)HttpStatusCode.BadRequest,
                        Success = false,
                        Msg = "user does not exist!"
                    });
                }

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
        [HttpPost("beneficiaries")]
        //[AllowAnonymous]
        public MessageModel<UserBeneficiaryDTO> AddOwnBeneficiarsInfo([FromBody]OftenBeneficiaryRequest oftenBeneficiarRequest)
        {
            var userId = long.Parse(User.FindFirstValue("id"));
            OftenBeneficiary OftenBeneficiary = mapper.Map<OftenBeneficiary>(oftenBeneficiarRequest);
            OftenBeneficiary.UserId = userId;
            var payeeType = payeeTypeService.GetPayeeTypeByType((PayeeTypeEnum)oftenBeneficiarRequest.PayeeType);
            OftenBeneficiary.PayeeTypeId = payeeType.Id;
            beneficiarService.AddBeneficiar(OftenBeneficiary);
            UserBeneficiaryDTO userBeneficiarDTO = mapper.Map<UserBeneficiaryDTO>(OftenBeneficiary);



            return new MessageModel<UserBeneficiaryDTO>
            {
                Data = userBeneficiarDTO
            };
        }

        /// <summary>
        /// 修改使用者的常用收款人資訊
        /// </summary>
        [HttpPut("beneficiaries/{id}")]
        [Authorize]
        public ActionResult<MessageModel<UserBeneficiaryDTO>> ModifyOwnBeneficiarsInfo([FromRoute, SwaggerParameter("id,可從/user/beneficiars取得", Required = true)] int id,[FromBody]OftenBeneficiaryRequest oftenBeneficiarRequest)
        {
            var userId = long.Parse(User.FindFirstValue("id"));
            OftenBeneficiary Beneficiary = beneficiarService.GetBeneficiarById(id);
            if (Beneficiary == null)
            {
                return NotFound();
            }
            if (userId != Beneficiary.UserId)
            {
                return Unauthorized(new MessageModel<UserBeneficiaryDTO>()
                {
                    Success = false,
                    Msg = "Unauthorized"
                });
                
            }

            OftenBeneficiary modifiedBeneficiar = mapper.Map<OftenBeneficiary>(oftenBeneficiarRequest);
            modifiedBeneficiar.Id = id;
            var payeeType = payeeTypeService.GetPayeeTypeByType((PayeeTypeEnum)oftenBeneficiarRequest.PayeeType);
            modifiedBeneficiar.PayeeTypeId = payeeType.Id;
            
            beneficiarService.ModifyBeneficiar(modifiedBeneficiar, Beneficiary);
            UserBeneficiaryDTO userBeneficiarDTO = mapper.Map<UserBeneficiaryDTO>(Beneficiary);



            return new MessageModel<UserBeneficiaryDTO>
            {
                Data = userBeneficiarDTO
            };
        }

        /// <summary>
        /// 刪除使用者的常用收款人資訊
        /// </summary>
        [HttpDelete("beneficiaries/{id}")]
        [Authorize]
        public ActionResult<MessageModel<UserBeneficiaryDTO>> DeleteOwnBeneficiarsInfo([FromRoute, SwaggerParameter("id,可從/user/beneficiars取得", Required = true)] int id)
        {
            var userId = long.Parse(User.FindFirstValue("id"));
            OftenBeneficiary Beneficiary = beneficiarService.GetBeneficiarById(id);
            if(Beneficiary == null)
            {
                return NotFound();
            }
            if (userId != Beneficiary.UserId)
            {
                return Unauthorized(new MessageModel<UserBeneficiaryDTO>()
                {
                    Success = false,
                    Msg = "Unauthorized"
                });

            }
            beneficiarService.DeleteBeneficiar(Beneficiary);

            return new MessageModel<UserBeneficiaryDTO>() {  };
            
        }

        /// <summary>
        /// 取得使用者的常用收款人資訊
        /// </summary>
        [HttpGet("beneficiaries")]
        [Authorize]
        public MessageModel<List<UserBeneficiaryDTO>> GetOwnBeneficiarsInfo()
        {
            var userId = long.Parse(User.FindFirstValue("id"));
            List<OftenBeneficiary> oftenBeneficiars = beneficiarService.GetAllBeneficiars(userId);
            List<UserBeneficiaryDTO> userBeneficiarDTOs = mapper.Map<List<UserBeneficiaryDTO>>(oftenBeneficiars);


            return new MessageModel<List<UserBeneficiaryDTO>>
            {
                Data = userBeneficiarDTOs
            };
        }


    }
}
