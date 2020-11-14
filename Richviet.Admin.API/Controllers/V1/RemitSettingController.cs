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

namespace Richviet.Admin.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("admin/v{version:apiVersion}/remitsetting")]
    [ApiController]
    public class RemitSettingController : ControllerBase
    {
        private readonly ILogger Logger;
        private readonly IMapper mapper;
        private readonly IRemitSettingService remitSettingService;

        public RemitSettingController(ILogger<RemitSettingController> logger, IMapper mapper, IRemitSettingService remitSettingService)
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.mapper = mapper;
            this.remitSettingService = remitSettingService;
        }

        /// <summary>
        /// 取得匯款相關設定列表(min,max)
        /// </summary>
        [HttpGet]
        public MessageModel<List<EditRemitSettingDTO>> GetRemitSettingList()
        {
            List<BussinessUnitRemitSetting> settings = remitSettingService.GetRemitSettingList();
            List<EditRemitSettingDTO> settingDTOs = mapper.Map<List<EditRemitSettingDTO>>(settings);

            return new MessageModel<List<EditRemitSettingDTO>>
            {
                Data = settingDTOs
            };
        }

        /// <summary>
        /// 新增匯款相關設定
        /// </summary>
        [HttpPost]
        public MessageModel<EditRemitSettingDTO> AddRemitSetting(AddRemitSettingRequest request)
        {
            BussinessUnitRemitSetting rmitSetting = new BussinessUnitRemitSetting
            {
                Country = request.Country,
                RemitMin = request.RemitMin,
                RemitMax = request.RemitMax,
            };
            bool result = remitSettingService.AddRemitSetting(rmitSetting);
            EditRemitSettingDTO rmitSettingDTO = mapper.Map<EditRemitSettingDTO>(rmitSetting);
            return new MessageModel<EditRemitSettingDTO>
            {
                Success = result,
                Msg = result ? "" : "Add Fail",
                Data = rmitSettingDTO,
            };
        }

        /// <summary>
        /// 刪除匯款相關設定
        /// </summary>
        [HttpDelete("{id}")]
        public MessageModel<EditRemitSettingDTO> DeleteRemitSetting([FromRoute, SwaggerParameter("id,可從/remitsetting取得", Required = true)] int id)
        {
            bool result = remitSettingService.DeleteRemitSetting(id);
            return new MessageModel<EditRemitSettingDTO>
            {
                Success = result,
                Msg = result ? "" : "Delete Fail"
            };
        }

        /// <summary>
        /// 修改匯款相關設定
        /// </summary>
        [HttpPut("{id}")]
        public MessageModel<EditRemitSettingDTO> ModifyRemitSetting([FromRoute, SwaggerParameter("id,可從/remitsetting取得", Required = true)] int id, [FromBody] ModifyRemitSettingRequest request)
        {
            BussinessUnitRemitSetting rmitSetting = new BussinessUnitRemitSetting
            {
                Id = id,
                Country = request.Country,
                RemitMin = request.RemitMin,
                RemitMax = request.RemitMax,
            };
            bool result = remitSettingService.ModifyRemitSetting(rmitSetting);
            EditRemitSettingDTO rmitSettingDTO = mapper.Map<EditRemitSettingDTO>(rmitSetting);
            return new MessageModel<EditRemitSettingDTO>
            {
                Success = result,
                Msg = result ? "" : "Modify Fail",
                Data = rmitSettingDTO
            };
        }
    }
}
