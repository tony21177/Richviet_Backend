using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Richviet.API.DataContracts.Dto;
using Richviet.API.DataContracts.Responses;
using Richviet.Services.Contracts;
using Richviet.Services.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Richviet.API.Controllers.V1
{

    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/currency")]
    [ApiController]
    public class CurrencyController : Controller
    {
        private readonly ILogger Logger;
        private readonly ICurrencyService currencyService;
        private readonly IExchangeRateService exchangeRateService;
        private readonly IMapper mapper;

        public CurrencyController(ILogger<CurrencyController> logger,ICurrencyService currencyService, IExchangeRateService exchangeRateService, IMapper mapper)
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.currencyService = currencyService;
            this.exchangeRateService = exchangeRateService;
            this.mapper = mapper;
        }

        /// <summary>
        /// 取得收款國家可選擇幣別和其手續費資料
        /// </summary>
        [HttpGet("{country}")]
        [AllowAnonymous]
        public MessageModel<List<CurrencyInfoDTO>> GetCurrencyInfo([FromRoute, SwaggerParameter("國家 e.g. VN ", Required = true)] string country)
        {
            Logger.LogInformation(country);
            List<CurrencyCode> currencyCodes = currencyService.GetCureencyByCountry(country.ToUpper());
            List<CurrencyInfoDTO> currencyInfoDTOs = mapper.Map<List<CurrencyInfoDTO>>(currencyCodes);



            return new MessageModel<List<CurrencyInfoDTO>>
            {
                Data = currencyInfoDTOs
            };

        }

        /// <summary>
        /// 取得台幣對各收款幣別的匯率
        /// </summary>
        [HttpGet("exchangeRate")]
        [AllowAnonymous]
        public MessageModel<List<ExchangeRateDTO>> GetExchangeRate()
        {
            List<ExchangeRate> exchangeRates = exchangeRateService.GetExchangeRate();
            List<ExchangeRateDTO> exchangeRateDTOs = mapper.Map<List<ExchangeRateDTO>>(exchangeRates);



            return new MessageModel<List<ExchangeRateDTO>>
            {
                Data = exchangeRateDTOs
            };



        }



    }
}
