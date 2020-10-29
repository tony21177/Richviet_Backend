using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Richviet.API.DataContracts.Dto;
using Richviet.API.DataContracts.Requests;
using Richviet.API.DataContracts.Responses;

namespace Richviet.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/uploadPicture")]
    [ApiController]
    public class UploadPictureController : ControllerBase
    {
        private readonly ILogger logger;

        public UploadPictureController(ILogger<UploadPictureController> logger)
        {
            this.logger = logger;
        }


        /// <summary>
        /// 上傳照片 content-type為multipart/formdata且body的ImageType為0:及時照,1:簽名照
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<MessageModel<UploadedFileDTO>>> UploadPicture([FromForm] CommonFileRequest file) {
            //Logger.LogInformation(file.ImageType.ToString());

            return Ok(new MessageModel<Object>
            {
                Data = new UploadedFileDTO
                {
                    FileName = "0" +"_"+ "ARC0000001" +"_"+ DateTimeOffset.Now.ToUnixTimeSeconds().ToString()
                }
        
            });
        }

    }
}
