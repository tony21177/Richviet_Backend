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
using Frontend.DB.EF.Models;
using Richviet.Tools.Utility;
using Microsoft.AspNetCore.Hosting;
using RemitRecords.Domains.RemitRecords.Constants;

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
        private readonly IRemitRecordService remitRecordService;
        private readonly IUploadPic uploadPic;
        public UploadPictureController(IUserService userService, IRemitRecordService remitRecordService, ILogger<UploadPictureController> logger, FolderHandler folderHandler, IUploadPic uploadPic
            )
        {
            this.logger = logger;
            this.folderHandler = folderHandler;
            this.userService = userService;
            this.remitRecordService = remitRecordService;
            this.uploadPic = uploadPic;
        }


        /// <summary>
        /// 上傳照片 content-type為multipart/formdata且body的ImageType為0:及時照,1:簽名照,2:證件正面照,3:證件反面照,
        /// 當imageType為0或1時,須帶入匯款的id for draftRemitRecordId
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<MessageModel<UploadedFileDTO>>> UploadPicture([FromForm] CommonFileRequest file) {
            //Logger.LogInformation(file.ImageType.ToString());
            var userId = long.Parse(User.FindFirstValue("id"));
            string fileName = null;
            UserArc userArc = userService.GetUserArcById(userId);
            // for member register process
            if (file.ImageType == (byte)PictureTypeEnum.Front || file.ImageType == (byte)PictureTypeEnum.Back)
            {
                if (userArc.KycStatus != (short)KycStatusEnum.DRAFT_MEMBER)
                return BadRequest(new MessageModel<UploadedFileDTO>
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Success = false,
                    Msg = "Can not upload!"
                });

                fileName = await uploadPic.SavePic(userArc,file.ImageType,file.Image);
                // pic for register process
            
                userService.UpdatePicFileNameOfUserInfo(userArc, (PictureTypeEnum)file.ImageType, fileName);
            }
            // for draft remit apply
            if (file.ImageType == (byte)PictureTypeEnum.Instant || file.ImageType == (byte)PictureTypeEnum.Signature)
            {
                if (userArc.KycStatus != (short)KycStatusEnum.PASSED_KYC_FORMAL_MEMBER)
                    return BadRequest(new MessageModel<UploadedFileDTO>
                    {
                        Status = (int)HttpStatusCode.BadRequest,
                        Success = false,
                        Msg = "You are not formal member,can not upload!"
                    });
                // check draft remit record exist
                if(file.DraftRemitRecordId == null)
                {
                    return BadRequest(new MessageModel<UploadedFileDTO>
                    {
                        Status = (int)HttpStatusCode.BadRequest,
                        Success = false,
                        Msg = "draftRemitRecordId is required!"
                    });

                }
                RemitRecord draftRemitRecord = remitRecordService.GetRemitRecordById((long)file.DraftRemitRecordId);
                if (draftRemitRecord == null || draftRemitRecord.TransactionStatus != (short) RemitTransactionStatusEnum.Draft)
                {
                    return BadRequest(new MessageModel<UploadedFileDTO>
                    {
                        Status = (int)HttpStatusCode.BadRequest,
                        Success = false,
                        Msg = "the draft Remit Record does not exist!"
                    });
                }

                fileName = await uploadPic.SavePic(userArc, file.ImageType, file.Image);
                userService.UpdatePicFileNameOfDraftRemit(draftRemitRecord, (PictureTypeEnum)file.ImageType, fileName);

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
