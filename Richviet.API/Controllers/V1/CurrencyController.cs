using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Richviet.API.DataContracts.Dto;
using Richviet.API.DataContracts.Responses;

namespace Richviet.API.Controllers.V1
{

    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/currency")]
    [ApiController]
    public class CurrencyController : Controller
    {


        public CurrencyController()
        {
        }

        /// <summary>
        /// 取得台幣對各收款幣別的匯率
        /// </summary>
        [HttpGet("exchangeRate")]
        public MessageModel<ExchangeRateDTO> getExchangeRate()
        {
            
            return new MessageModel<ExchangeRateDTO>
            {
                Data = new ExchangeRateDTO()
                {
                    currencyName = "USD",
                    rate = 30
                }
            };
        }

    }
}
