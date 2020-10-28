using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Richviet.API.DataContracts;
using Richviet.API.DataContracts.Dto;
using Richviet.API.DataContracts.Responses;
using Richviet.Services.Contracts;
using Richviet.Services.Models;

#pragma warning disable    1591
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

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        /// 取得登入的使用者相關資訊
        /// </summary>
        [HttpGet("info")]
        public MessageModel<UserInfoDTO> getOwnUserInfo()
        {
            //int id = User.Claims
            //UserInfoView userInfo = userService.GetUserById();
            return new MessageModel<UserInfoDTO>
            {
                Data = new UserInfoDTO()
            };
        }

        /// <summary>
        /// 取得使用者的常用收款人資訊
        /// </summary>
        [HttpGet("beneficiars")]
        [AllowAnonymous]
        public MessageModel<UserBeneficiarsDTO []> getOwnBeneficiarsInfo()
        {
            
            return new MessageModel<UserBeneficiarsDTO []>
            {
                Data = new UserBeneficiarsDTO[2]
                {
                    new UserBeneficiarsDTO
                    {
                        Id = 1,
                        Name = "爸爸",
                        PayeeAddress = "***************932",
                        PayeeId = "",
                        Note = "爸爸帳號",
                        VietName = "第一銀行",
                        EnName = "First Bank",
                        TwName = "第一銀行",
                        ArcNo = "ARC123456",
                        Type = 0
                    },
                    new UserBeneficiarsDTO
                    {
                        Id = 1,
                        Name = "媽",
                        PayeeAddress = "***************552",
                        PayeeId = "",
                        Note = "媽的帳號",
                        VietName = "國泰銀行",
                        EnName = "Guo Tai Bank",
                        TwName = "國泰銀行",
                        ArcNo = "ARC444678",
                        Type = 0
                    }
                }
            };
        }


    }
}
