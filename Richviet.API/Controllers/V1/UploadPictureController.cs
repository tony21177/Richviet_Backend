using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Richviet.API.DataContracts.Dto;
using Richviet.API.DataContracts.Requests;
using Richviet.API.DataContracts.Responses;
using Richviet.Services.Constants;
using Richviet.Services.Contracts;
using Richviet.Services.Models;
using Richviet.Tools.Utility;

namespace Richviet.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/uploadPicture")]
    [ApiController]
    public class UploadPictureController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly FolderHandler folderHandler;
        private readonly IUserService userService;
        private readonly IUploadPic uploadPic;

        public UploadPictureController(IUserService userService,ILogger<UploadPictureController> logger, FolderHandler folderHandler, IUploadPic uploadPic)
        {
            this.logger = logger;
            this.folderHandler = folderHandler;
            this.userService = userService;
            this.uploadPic = uploadPic;
        }


        /// <summary>
        /// 上傳照片 content-type為multipart/formdata且body的ImageType為0:及時照,1:簽名照,2:證件正面照,3:證件反面照
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<MessageModel<UploadedFileDTO>>> UploadPicture([FromForm] CommonFileRequest file) {
            //Logger.LogInformation(file.ImageType.ToString());
            var userId = int.Parse(User.FindFirstValue("id"));
            UserArc userArc = userService.GetUserArcById(userId);
            if (userArc.KycStatus != (byte)KycStatusEnum.DRAFT_MEMBER)
                return BadRequest(new MessageModel<UploadedFileDTO>
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Success = false,
                    Msg = "Can not upload!"
                });

            String fileName = await uploadPic.SavePic(userArc,file.ImageType,file.Image);
            if(file.ImageType == (byte) PictureTypeEnum.Front || file.ImageType == (byte)PictureTypeEnum.Back)
            {
                logger.LogInformation("update pic................................");
                userService.UpdatePicFileNameOfUserInfo(userArc, file.ImageType, fileName);
            }
            
            return Ok(new MessageModel<Object>
            {
                Data = new UploadedFileDTO
                {
                    FileName = fileName
                }
        
            });
        }

    }
}
