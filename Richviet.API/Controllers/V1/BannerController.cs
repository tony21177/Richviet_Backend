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
    [Route("api/v{version:apiVersion}/carousel")]
    [ApiController]
    public class BannerController : Controller
    {
        private readonly ILogger Logger;
        private readonly IMapper _mapper;
        private readonly IBannerService bannerService;

        public BannerController(ILogger<BankController> logger, IMapper mapper, IBannerService bannerService)
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._mapper = mapper;
            this.bannerService = bannerService;
        }

        /// <summary>
        /// 取得輪播照片連結
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public MessageModel<List<String>> GetCarousels()
        {

            List<String> list = bannerService.GetCaroudsels().Result;


            return new MessageModel<List<String>>
            {
                Data = list
            };
        }

    }
}
