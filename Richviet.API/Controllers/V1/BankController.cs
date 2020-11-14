using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Richviet.API.DataContracts.Dto;
using Richviet.API.DataContracts.Responses;
using Richviet.Services.Contracts;
using Frontend.DB.EF.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Richviet.API.Controllers.V1
{

    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/banks")]
    [ApiController]
    public class BankController : Controller
    {
        private readonly ILogger Logger;
        private readonly IMapper _mapper;
        private readonly IBankService _bankService;

        public BankController(ILogger<BankController> logger, IMapper mapper, IBankService bankService)
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._mapper = mapper;
            this._bankService = bankService;
        }

        /// <summary>
        /// 取得可收款銀行列表
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public MessageModel<List<BankDTO>> GetReceiveBanks()
        {

            List<ReceiveBank> receiveBanks = _bankService.GetReceiveBanks();
            List<BankDTO> bankDTOs =  _mapper.Map<List<BankDTO>>(receiveBanks);


            return new MessageModel<List<BankDTO>>
            {
                Data = bankDTOs
            };
        }

    }
}
