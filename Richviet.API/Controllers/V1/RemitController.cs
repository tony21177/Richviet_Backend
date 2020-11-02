using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Richviet.API.DataContracts.Dto;
using Richviet.API.DataContracts.Requests;
using Richviet.API.DataContracts.Responses;
using Richviet.Services.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;

#pragma warning disable    1591
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

        public RemitController(ILogger<RemitController> logger)
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// 取得服務所在國家的匯款相關設定(min,max)
        /// </summary>
        [HttpGet("settings/{country}")]
        [AllowAnonymous]
        public MessageModel<RemitSettingDTO> GetCurrencyInfo([FromRoute, SwaggerParameter("國家 e.g. TW ", Required = true)] string country)
        {
            Logger.LogInformation(country);

            return new MessageModel<RemitSettingDTO>
            {
                Data = new RemitSettingDTO {
                    country = "TW",
                    remitMin = 1000,
                    remitMax = 30000
                }
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
        [AllowAnonymous]
        public ActionResult<MessageModel<RemitRecordDTO>> ApplyRemitRecord([FromBody] RemitRequest remitRequest)
        {
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
        [HttpGet("remitRecords/{arcNo}")]
        [AllowAnonymous]
        public ActionResult<MessageModel<RemitRecordDTO []>> GetRemitRecords([FromRoute, SwaggerParameter("ARC No.", Required = true)] string arcNo)
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


    }
}
