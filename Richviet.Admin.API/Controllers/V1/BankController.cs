using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Richviet.Admin.API.DataContracts.Dto;
using Richviet.Admin.API.DataContracts.Requests;
using Richviet.Admin.API.DataContracts.Responses;
using Richviet.Services.Contracts;
using Frontend.DB.EF.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Richviet.Admin.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("admin/v{version:apiVersion}/bank")]
    [ApiController]
    public class BankController : ControllerBase
    {
        private readonly ILogger Logger;
        private readonly IMapper mapper;
        private readonly IBankService bankService;

        public BankController(ILogger<BankController> logger, IMapper mapper, IBankService bankService)
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.mapper = mapper;
            this.bankService = bankService;
        }

        /// <summary>
        /// 取得可收款銀行列表
        /// </summary>
        [HttpGet]
        public MessageModel<List<EditBankDTO>> GetReceiveBanks()
        {

            List<ReceiveBank> receiveBanks = bankService.GetReceiveBanks();
            List<EditBankDTO> bankDTOs = mapper.Map<List<EditBankDTO>>(receiveBanks);


            return new MessageModel<List<EditBankDTO>>
            {
                Data = bankDTOs
            };
        }

        /// <summary>
        /// 新增可收款銀行
        /// </summary>
        [HttpPost]
        public MessageModel<EditBankDTO> AddReceiveBank(AddReceiveBankRequest request)
        {
            ReceiveBank bank = new ReceiveBank
            {
                SwiftCode = request.SwiftCode,
                Code = request.Code,
                VietName = request.VietName,
                EnName = request.EnName,
                TwName = request.TwName,
                SortNum = request.SortNum,
            };
            bool result = bankService.AddReceiveBank(bank);
            EditBankDTO bankDTO = mapper.Map<EditBankDTO>(bank);
            return new MessageModel<EditBankDTO>
            {
                Success = result,
                Msg = result ? "" : "Add Fail",
                Data = bankDTO
            };
        }

        /// <summary>
        /// 刪除可收款銀行
        /// </summary>
        [HttpDelete("{id}")]
        public MessageModel<EditBankDTO> DeleteReceiveBank([FromRoute, SwaggerParameter("id,可從/bank取得", Required = true)] int id)
        {
            bool result = bankService.DeleteReceiveBank(id);
            return new MessageModel<EditBankDTO>
            {
                Success = result,
                Msg = result ? "" : "Delete Fail"
            };
        }

        /// <summary>
        /// 修改可收款銀行
        /// </summary>
        [HttpPut("{id}")]
        public MessageModel<EditBankDTO> ModifyReceiveBank([FromRoute, SwaggerParameter("id,可從/bank取得", Required = true)] int id, [FromBody] ModifyReceiveBankRequest request)
        {
            ReceiveBank bank = new ReceiveBank
            {
                Id = id,
                SwiftCode = request.SwiftCode,
                Code = request.Code,
                VietName = request.VietName,
                EnName = request.EnName,
                TwName = request.TwName,
                SortNum = request.SortNum,
            };
            bool result = bankService.ModifyReceiveBank(bank);
            EditBankDTO bankDTO = mapper.Map<EditBankDTO>(bank);
            return new MessageModel<EditBankDTO>
            {
                Success = result,
                Msg = result ? "" : "Modify Fail",
                Data = bankDTO
            };
        }
    }
}
