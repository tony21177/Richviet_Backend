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
    [Route("admin/v{version:apiVersion}/relation")]
    [ApiController]
    public class RelationController : ControllerBase
    {
        private readonly ILogger Logger;
        private readonly IMapper mapper;
        private readonly IPayeeRelationService payeeRelationService;

        public RelationController(ILogger<RelationController> logger, IMapper mapper, IPayeeRelationService payeeRelationService)
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.mapper = mapper;
            this.payeeRelationService = payeeRelationService;
        }

        /// <summary>
        /// 取得與收款人所有可選擇關係列表
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public MessageModel<List<EditRelationDTO>> GetRelations()
        {
            List<PayeeRelationType> payeeRelationTypes = payeeRelationService.GetPayeeRelations();
            List<EditRelationDTO> relationDTOs = mapper.Map<List<EditRelationDTO>>(payeeRelationTypes);

            return new MessageModel<List<EditRelationDTO>>
            {
                Data = relationDTOs
            };
        }

        /// <summary>
        /// 新增收款人關係
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        public MessageModel<EditRelationDTO> AddRelation(AddPayeeRelationTypeRequest request)
        {
            PayeeRelationType payeeRelationType = new PayeeRelationType
            {
                Type = request.Type,
                Description = request.Description,
            };
            bool result = payeeRelationService.AddPayeeRelation(payeeRelationType);
            EditRelationDTO relationDTO = mapper.Map<EditRelationDTO>(payeeRelationType);
            return new MessageModel<EditRelationDTO>
            {
                Success = result,
                Msg = result ? "" : "Add Fail",
                Data = relationDTO
            };
        }

        /// <summary>
        /// 刪除收款人關係
        /// </summary>
        [HttpDelete("{id}")]
        [AllowAnonymous]
        public MessageModel<EditRelationDTO> DeleteRelation([FromRoute, SwaggerParameter("id,可從/relation取得", Required = true)] int id)
        {
            bool result = payeeRelationService.DeletePayeeRelation(id);
            return new MessageModel<EditRelationDTO>
            {
                Success = result,
                Msg = result ? "" : "Delete Fail"
            };
        }

        /// <summary>
        /// 修改收款人關係
        /// </summary>
        [HttpPut("{id}")]
        [AllowAnonymous]
        public MessageModel<EditRelationDTO> ModifyRelation([FromRoute, SwaggerParameter("id,可從/relation取得", Required = true)] int id, [FromBody] ModifyPayeeRelationTypeRequest request)
        {
            PayeeRelationType payeeRelationType = new PayeeRelationType
            {
                Id = id,
                Type = request.Type,
                Description = request.Description,
            };
            bool result = payeeRelationService.ModifyPayeeRelation(payeeRelationType);
            EditRelationDTO relationDTO = mapper.Map<EditRelationDTO>(payeeRelationType);
            return new MessageModel<EditRelationDTO>
            {
                Success = result,
                Msg = result ? "" : "Modify Fail",
                Data = relationDTO
            };
        }
    }
}
