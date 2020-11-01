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
    [Route("api/v{version:apiVersion}/relations")]
    [ApiController]
    public class RelationController : Controller
    {
        private readonly ILogger Logger;
        private readonly IMapper _mapper;
        private readonly IPayeeRelationService _payeeRelationService;

        public RelationController(ILogger<RelationController> logger, IPayeeRelationService payeeRelationService, IMapper mapper)
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._mapper = mapper;
            this._payeeRelationService = payeeRelationService;
        }

        /// <summary>
        /// 取得與收款人所有可選擇關係列表
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public MessageModel<List<RelationDTO>> GetRelations()
        {
            List<PayeeRelationType> payeeRelationTypes = _payeeRelationService.GetPayeeRelations();
            List<RelationDTO> relationDTOs = _mapper.Map<List<RelationDTO>>(payeeRelationTypes);
            
            return new MessageModel<List<RelationDTO>>
            {
                Data = relationDTOs
            };
        }

    }
}
