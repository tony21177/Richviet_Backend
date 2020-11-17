
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
            var userId = long.Parse(User.FindFirstValue("id"));
            OftenBeneficiar oftenBeneficiar = mapper.Map<OftenBeneficiar>(oftenBeneficiarRequest);
            oftenBeneficiar.UserId = userId;
            var payeeType = payeeTypeService.GetPayeeTypeByType((PayeeTypeEnum)oftenBeneficiarRequest.PayeeType);
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
        [Authorize]
        public ActionResult<MessageModel<UserBeneficiarDTO>> ModifyOwnBeneficiarsInfo([FromRoute, SwaggerParameter("id,可從/user/beneficiars取得", Required = true)] int id,[FromBody]OftenBeneficiarRequest oftenBeneficiarRequest)
        {
            var userId = long.Parse(User.FindFirstValue("id"));
            OftenBeneficiar beneficiar = beneficiarService.GetBeneficiarById(id);
            if (beneficiar == null)
            {
                return NotFound();
            }
            if (userId != beneficiar.UserId)
            {
                return Unauthorized(new MessageModel<UserBeneficiarDTO>()
                {
                    Success = false,
                    Msg = "Unauthorized"
                });
                
            }

            OftenBeneficiar modifiedBeneficiar = mapper.Map<OftenBeneficiar>(oftenBeneficiarRequest);
            modifiedBeneficiar.Id = id;
            var payeeType = payeeTypeService.GetPayeeTypeByType((PayeeTypeEnum)oftenBeneficiarRequest.PayeeType);
            modifiedBeneficiar.PayeeTypeId = payeeType.Id;
            
            beneficiarService.ModifyBeneficiar(modifiedBeneficiar, beneficiar);
            UserBeneficiarDTO userBeneficiarDTO = mapper.Map<UserBeneficiarDTO>(beneficiar);



            return new MessageModel<UserBeneficiarDTO>
            {
                Data = userBeneficiarDTO
            };
        }

        /// <summary>
        /// 刪除使用者的常用收款人資訊
        /// </summary>
        [HttpDelete("beneficiars/{id}")]
        [Authorize]
        public ActionResult<MessageModel<UserBeneficiarDTO>> DeleteOwnBeneficiarsInfo([FromRoute, SwaggerParameter("id,可從/user/beneficiars取得", Required = true)] int id)
        {
            var userId = long.Parse(User.FindFirstValue("id"));
            OftenBeneficiar beneficiar = beneficiarService.GetBeneficiarById(id);
            if(beneficiar == null)
            {
                return NotFound();
            }
            if (userId != beneficiar.UserId)
            {
                return Unauthorized(new MessageModel<UserBeneficiarDTO>()
                {
                    Success = false,
                    Msg = "Unauthorized"
                });

            }
            beneficiarService.DeleteBeneficiar(beneficiar);

            return new MessageModel<UserBeneficiarDTO>() {  };
            
        }

        /// <summary>
        /// 取得使用者的常用收款人資訊
        /// </summary>
        [HttpGet("beneficiars")]
        [Authorize]
        public MessageModel<List<UserBeneficiarDTO>> GetOwnBeneficiarsInfo()
        {
            var userId = long.Parse(User.FindFirstValue("id"));
            List<OftenBeneficiar> oftenBeneficiars = beneficiarService.GetAllBeneficiars(userId);
            List<UserBeneficiarDTO> userBeneficiarDTOs = mapper.Map<List<UserBeneficiarDTO>>(oftenBeneficiars);


            return new MessageModel<List<UserBeneficiarDTO>>
            {
                Data = userBeneficiarDTOs
            };
        }


    }
}
