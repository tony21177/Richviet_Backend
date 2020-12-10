using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Richviet.Admin.API.DataContracts.Dto;
using Richviet.Admin.API.DataContracts.Requests;
using Richviet.Admin.API.DataContracts.Responses;
using Richviet.Services.Contracts;
using Frontend.DB.EF.Models;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;

namespace Richviet.Admin.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("admin/v{version:apiVersion}/currency")]
    [ApiController]
    [Authorize(Roles = "adminManager")]
    public class CurrencyController : ControllerBase
    {
        private readonly ILogger Logger;
        private readonly IMapper mapper;
        private readonly ICurrencyService currencyService;

        public CurrencyController(ILogger<CurrencyController> logger, IMapper mapper, ICurrencyService currencyService)
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.mapper = mapper;
            this.currencyService = currencyService;
        }

        /// <summary>
        /// 取得收款國家幣別和手續費資料列表
        /// </summary>
        [HttpGet]
        public MessageModel<List<EditCurrencyInfoDTO>> GetCurrencyInfo()
        {
            List<CurrencyCode> currencyCodes = currencyService.GetCurrencyList();
            List<EditCurrencyInfoDTO> currencyInfoDTOs = mapper.Map<List<EditCurrencyInfoDTO>>(currencyCodes);
            return new MessageModel<List<EditCurrencyInfoDTO>>
            {
                Data = currencyInfoDTOs
            };

        }

        /// <summary>
        /// 新增收款國家幣別和手續費資料
        /// </summary>
        [HttpPost]
        public MessageModel<EditCurrencyInfoDTO> AddCurrencyInfo(AddCurrencyCodeRequest request)
        {
            CurrencyCode currency = new CurrencyCode
            {
                CurrencyName = request.CurrencyName,
                Country = request.Country,
                Fee = request.Fee,
                FeeType = (byte)request.FeeType
            };
            bool result = currencyService.AddCurrency(currency);
            EditCurrencyInfoDTO currencyDTO = mapper.Map<EditCurrencyInfoDTO>(currency);
            return new MessageModel<EditCurrencyInfoDTO>
            {
                Success = result,
                Msg = result ? "" : "Add Fail",
                Data = currencyDTO
            };
        }

        /// <summary>
        /// 刪除收款國家幣別和手續費資料
        /// </summary>
        [HttpDelete("{id}")]
        public MessageModel<EditCurrencyInfoDTO> DeleteCurrencyInfo([FromRoute, SwaggerParameter("id,可從/currency取得", Required = true)] int id)
        {
            bool result = currencyService.DeleteCurrency(id);
            return new MessageModel<EditCurrencyInfoDTO>
            {
                Success = result,
                Msg = result ? "" : "Delete Fail"
            };
        }

        /// <summary>
        /// 修改收款國家幣別和手續費資料
        /// </summary>
        [HttpPut("{id}")]
        public MessageModel<EditCurrencyInfoDTO> ModifyCurrencyInfo([FromRoute, SwaggerParameter("id,可從/currency取得", Required = true)] int id, [FromBody] ModifyCurrencyCodeRequest request)
        {
            CurrencyCode currency = new CurrencyCode
            {
                Id = id,
                CurrencyName = request.CurrencyName,
                Country = request.Country,
                Fee = request.Fee,
                FeeType = (byte)request.FeeType
            };
            bool result = currencyService.ModifyCurrency(currency);
            EditCurrencyInfoDTO currencyDTO = mapper.Map<EditCurrencyInfoDTO>(currency);
            return new MessageModel<EditCurrencyInfoDTO>
            {
                Success = result,
                Msg = result ? "" : "Modify Fail",
                Data = currencyDTO
            };
        }
    }
}
