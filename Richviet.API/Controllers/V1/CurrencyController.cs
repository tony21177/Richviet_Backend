using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Richviet.API.DataContracts.Dto;
using Richviet.API.DataContracts.Responses;
using Swashbuckle.AspNetCore.Annotations;
using System;

namespace Richviet.API.Controllers.V1
{

    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/currency")]
    [ApiController]
    public class CurrencyController : Controller
    {
        private readonly ILogger Logger;

        public CurrencyController(ILogger<CurrencyController> logger)
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// 取得收款國家可選擇幣別和其手續費資料
        /// </summary>
        [HttpGet("{country}")]
        [AllowAnonymous]
        public MessageModel<CurrencyInfoDTO []> GetCurrencyInfo([FromRoute, SwaggerParameter("國家 e.g. VN ", Required = true)] string country)
        {
            Logger.LogInformation(country);
            
            return new MessageModel<CurrencyInfoDTO []>
            {
                Data = new CurrencyInfoDTO[2]{
                    new CurrencyInfoDTO
                    {
                        currencyName = "VND",
                        country = "VN",
                        feeType = 0,
                        fee = 100
                    },new CurrencyInfoDTO
                    {
                        currencyName = "USD",
                        country = "VN",
                        feeType = 0,
                        fee = 100
                    }
                }
            };

        }

        /// <summary>
        /// 取得台幣對各收款幣別的匯率
        /// </summary>
        [HttpGet("exchangeRate")]
        [AllowAnonymous]
        public MessageModel<ExchangeRateDTO []> GetExchangeRate()
        {
            
            return new MessageModel<ExchangeRateDTO []>
            {
                    Data = new ExchangeRateDTO[2]
                    {
                        new ExchangeRateDTO{
                            currencyName = "USD",
                            rate = 30
                        },new ExchangeRateDTO{
                            currencyName = "VND",
                            rate = 800
                        }
                    }
             
            };
        }



    }
}
