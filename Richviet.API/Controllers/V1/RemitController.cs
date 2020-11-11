using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Richviet.API.DataContracts.Dto;
using Richviet.API.DataContracts.Requests;
using Richviet.API.DataContracts.Responses;
using Richviet.Services.Constants;
using Richviet.Services.Contracts;
using Richviet.Services.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Net;
using System.Security.Claims;

#pragma warning disable 1591
namespace Richviet.API.Controllers.V1
{

    /// <summary>
    /// 匯款流程相關API
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/remit")]
    [ApiController]
    public class RemitController : Controller
    {
        private readonly ILogger Logger;

        private readonly IMapper _mapper;

        private readonly IUserService userService;

        private readonly IRemitSettingService remitSettingService;

        private readonly IBeneficiarService beneficiarService;

        private readonly IUploadPic uploadPicService;

        public RemitController(ILogger<RemitController> logger, IMapper mapper, IRemitSettingService remitSettingService, IBeneficiarService beneficiarService
            , IUserService userService, IUploadPic uploadPicService)
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._mapper = mapper;
            this.userService = userService;
            this.remitSettingService = remitSettingService;
            this.beneficiarService = beneficiarService;
            this.uploadPicService = uploadPicService;
        }

        /// <summary>
        /// 取得服務所在國家的匯款相關設定(min,max)
        /// </summary>
        [HttpGet("settings/{country}")]
        [Authorize]
        public MessageModel<RemitSettingDTO> GetCurrencyInfo([FromRoute, SwaggerParameter("國家 e.g. TW ", Required = true)] string country)
        {
            Logger.LogInformation(country);
            BussinessUnitRemitSetting setting = remitSettingService.GetRemitSettingByCountry(country.ToUpper());
            RemitSettingDTO settingDTO = _mapper.Map<RemitSettingDTO>(setting);

            return new MessageModel<RemitSettingDTO>
            {
                Data = settingDTO
            };

        }

        /// <summary>
        /// 使用者擁有的優惠券
        /// </summary>
        [HttpGet("discount")]
        [AllowAnonymous]
        public MessageModel<UserRemitDiscountDTO[]> GetUserDiscount()
        {

            return new MessageModel<UserRemitDiscountDTO[]>
            {
                Data = new UserRemitDiscountDTO[2]
                {
                    new UserRemitDiscountDTO()
                    {
                        Id=2,
                        UserId=10,
                        EffectiveDate=new DateTime(2020,11,1),
                        ExpireDate=new DateTime(2021,3,31),
                        Value=50
                    },
                    new UserRemitDiscountDTO()
                    {
                        Id=3,
                        UserId=10,
                        EffectiveDate=new DateTime(2020,11,1),
                        ExpireDate=new DateTime(2021,3,31),
                        Value=100
                    }
                }
            };

        }

        /// <summary>
        /// 使用者送出匯款申請
        /// </summary>
        [HttpPost]
        [Authorize]
        public ActionResult<MessageModel<RemitRecordDTO>> ApplyRemitRecord([FromBody] RemitRequest remitRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            // check user arc status
            var userId = int.Parse(User.FindFirstValue("id"));
            UserArc userArc = userService.GetUserArcById(userId);

            if(userArc.KycStatus!= (byte)KycStatusEnum.PASSED_KYC)
            {
                return BadRequest(new MessageModel<RemitRecordDTO>
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Success = false,
                    Msg = "KYC process Has not been passed"

                });
            }

            // check amount
            if (CheckIfAmountOutOfRange(remitRequest.FromAmount,"TW")) 
                return BadRequest(new MessageModel<RemitRecordDTO>
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Success = false,
                    Msg = "amount out of range"

                });
            // check beneficiar
            OftenBeneficiar beneficiar= beneficiarService.GetBeneficiarById(remitRequest.BeneficiarId);
            if (beneficiar == null)
            {
                return BadRequest(new MessageModel<RemitRecordDTO>
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Success = false,
                    Msg = "wrong beneficiar"

                });
            }
            // check uploaded picture
            if(!uploadPicService.CheckUploadFileExistence(userArc, PictureTypeEnum.Instant, remitRequest.PhotoFilename)
                || !uploadPicService.CheckUploadFileExistence(userArc, PictureTypeEnum.Signature, remitRequest.SignatureFilename))
            {
                return BadRequest(new MessageModel<RemitRecordDTO>
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Success = false,
                    Msg = "uploaded pictures does not exists"

                });
            }

            return Ok(new MessageModel<RemitRecordDTO>
            {
                Data = new RemitRecordDTO() {
                    Id = 101,
                    CreateTime = DateTimeOffset.Now,
                    ToCurrency = "VND",
                    FromAmount = 10000,
                    ToAmount = 7960000,
                    TransactionExchangeRate = 800,
                    Fee = 150,
                    DiscountAmount = 100,
                    Name = "爸爸",
                    PayeeAddress = "XXXXXXXXXX456",
                    Type = 0,
                    TransactionStatus = 0
                }
            });
        }

        /// <summary>
        /// 取得該筆匯款紀錄的繳款碼
        /// </summary>
        /// 
        [HttpGet("payment/{id}")]
        [AllowAnonymous]
        public ActionResult<MessageModel<PaymentCodeDTO>> GetPaymentCode([FromRoute, SwaggerParameter("交易紀錄id", Required = true)] int id)
        {
            return Ok(new MessageModel<PaymentCodeDTO>()
            {
                Data = new PaymentCodeDTO()
                {
                    Code = "WEFQEWFEFQEQGRGRG009233"
                }
            });
        }

        /// <summary>
        /// 該會員的交易紀錄
        /// </summary>
        /// 
        [HttpGet("remitRecords")]
        [AllowAnonymous]
        public ActionResult<MessageModel<RemitRecordDTO []>> GetRemitRecords()
        {
            return Ok(new MessageModel<RemitRecordDTO[]>
            {
                Data =  new RemitRecordDTO[] {
                    new RemitRecordDTO()
                    {
                        Id = 101,
                        CreateTime = DateTimeOffset.Now,
                        ToCurrency = "VND",
                        FromAmount = 10000,
                        ToAmount = 7960000,
                        TransactionExchangeRate = 800,
                        Fee = 150,
                        DiscountAmount = 100,
                        Name = "爸爸",
                        PayeeAddress = "XXXXXXXXXX456",
                        Type = 0,
                        TransactionStatus = 0
                    }, new RemitRecordDTO()
                    {
                        Id = 102,
                        CreateTime = DateTimeOffset.Now,
                        ToCurrency = "USD",
                        FromAmount = 10000,
                        ToAmount = 348.25,
                        TransactionExchangeRate = 0.035,
                        Fee = 150,
                        DiscountAmount = 100,
                        Name = "爸爸",
                        PayeeAddress = "XXXXXXXXXX456",
                        Type = 0,
                        TransactionStatus = 1
                    }
                }
            });
        }


        private bool CheckIfAmountOutOfRange(int amount,string country)
        {
            BussinessUnitRemitSetting remitSetting = remitSettingService.GetRemitSettingByCountry(country);
            if (remitSetting == null) return true;
            if (amount < remitSetting.RemitMin || amount > remitSetting.RemitMax) return true;
            return false;
        }


    }
}
