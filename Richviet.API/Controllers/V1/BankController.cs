using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Richviet.API.DataContracts.Dto;
using Richviet.API.DataContracts.Responses;
using Richviet.Services.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;

namespace Richviet.API.Controllers.V1
{

    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/banks")]
    [ApiController]
    public class BankController : Controller
    {
        private readonly ILogger Logger;

        public BankController(ILogger<BankController> logger)
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// 取得可收款銀行列表
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public MessageModel<BankDTO[]> GetReceiveBanks()
        {
            
            
            return new MessageModel<BankDTO[]>
            {
                Data = new BankDTO[]{
                    new BankDTO
                    {
                        Id = 1,
                        SwiftCode = "UWCBTWTP006",
                        Code = "013",
                        VietName = "國泰銀行",
                        EnName = "國泰銀行",
                        TwName = "國泰銀行"

                    }
                }
            };
        }

    }
}
