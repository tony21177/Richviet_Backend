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
    [Route("api/v{version:apiVersion}/relations")]
    [ApiController]
    public class RelationController : Controller
    {
        private readonly ILogger Logger;

        public RelationController(ILogger<RelationController> logger)
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// 取得與收款人所有可選擇關係列表
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public MessageModel<RelationDTO[]> GetRelations()
        {
            
            
            return new MessageModel<RelationDTO[]>
            {
                Data = new RelationDTO[]{
                    new RelationDTO
                    {
                        Id = 1,
                        Type = 0,
                        Description = "父母"

                    },new RelationDTO
                    {
                        Id = 2,
                        Type = 1,
                        Description = "子女"

                    }
                }
            };
        }

    }
}
