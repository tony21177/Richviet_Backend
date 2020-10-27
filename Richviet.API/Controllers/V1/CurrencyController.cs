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
        /// 取得指定收款幣別的手續費
        /// </summary>
        [HttpGet("fee/{currencyName}")]
        [AllowAnonymous]
        public MessageModel<CurrencyFeeDTO> getCurrencyFee([FromRoute, SwaggerParameter("幣別 e.g. VND ", Required = true)] string currencyName)
        {
            Logger.LogInformation(currencyName);
            
            return new MessageModel<CurrencyFeeDTO>
            {
                Data = new CurrencyFeeDTO
                {
                    currencyName = "VND",
                    feeType = 0,
                    fee = 100
                }
            };

        }

        /// <summary>
        /// 取得台幣對各收款幣別的匯率
        /// </summary>
        [HttpGet("exchangeRate")]
        [AllowAnonymous]
        public MessageModel<ExchangeRateDTO> getExchangeRate()
        {
            
            return new MessageModel<ExchangeRateDTO>
            {
                Data = new ExchangeRateDTO()
                {
                    exchangeRates = new ExchangeRateDTO.CurrencyRate[2]
                    {
                        new ExchangeRateDTO.CurrencyRate{
                            currencyName = "USD",
                            rate = 30
                        },new ExchangeRateDTO.CurrencyRate{
                            currencyName = "VND",
                            rate = 800
                        }
                    }
                }
            };
        }



    }
}
