using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using RemitRecords.Domains.RemitRecords.Command.UseCase;
using Richviet.Admin.API.DataContracts.Requests;
using Richviet.Admin.API.DataContracts.Responses;
using Users.Domains.Users.Command.Request;
using Richviet.Services.Contracts;
using Frontend.DB.EF.Models;
using Richviet.Admin.API.DataContracts.Dto;
using Swashbuckle.AspNetCore.Annotations;
using Richviet.Services.Constants;
using Microsoft.AspNetCore.Authorization;

namespace Richviet.Admin.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("admin/remit/v{version:apiVersion}/review")]
    [ApiController]
    //[Authorize(Roles = "adminManager")]
    public class RemitReviewController : ControllerBase
    {

        private readonly RemitRecordAmlReviewer amlReviewer;
        private readonly RemitTransactionStatusModifier statusModifier;
        private readonly IRemitRecordService remitRecordService;
        private readonly IMapper _mapper;
        private readonly IUploadPic uploadPicService;
        private readonly IBankService bankService;
        private readonly INotificationService notificationService;

        public RemitReviewController(RemitRecordAmlReviewer amlReviewer, RemitTransactionStatusModifier statusModifier, IRemitRecordService remitRecordService, IMapper mapper
            , IUploadPic uploadPicService, IBankService bankService, INotificationService firebaseService)
        {
            this.amlReviewer = amlReviewer;
            this.statusModifier = statusModifier;
            this.remitRecordService = remitRecordService;
            this._mapper = mapper;
            this.uploadPicService = uploadPicService;
            this.bankService = bankService;
            this.notificationService = firebaseService;
        }


        [HttpPost("AmlReview")]
        public MessageModel<string> AmlReview(AmlReviewModifyRequest request)
        {
            RemitRecord record = remitRecordService.GetRemitRecordById(request.RecordId);

            if (request.IsAmlPass)
            {
                amlReviewer.AmlReviewPass(request.RecordId, request.Comment);
                notificationService.SaveAndSendNotification((int)record.UserId, "To be Paid", "Your remit application is waiting for payment", "en-US");
            }
            else
            {
                amlReviewer.AmlReviewFail(request.RecordId, request.Comment);
                notificationService.SaveAndSendNotification((int)record.UserId, "Remit had been rejected", "Your remit application was rejected", "en-US");
            }

            return new MessageModel<string>()
            {
                Data = "Aml status update"
            };
        }

        [HttpPost("Complete")]
        public MessageModel<string> TransSuccess(TransactionStatusModifyRequest request)
        {
            RemitRecord record = remitRecordService.GetRemitRecordById(request.RecordId);
            if (request.IsComplete)
            {
                statusModifier.RemitSuccess(request.RecordId, request.Comment);
                notificationService.SaveAndSendNotification((int)record.UserId, "Remit Complete", "Your remit application completes", "en-US");
            }
            else
            {
                statusModifier.RemitFail(request.RecordId, request.Comment);
                notificationService.SaveAndSendNotification((int)record.UserId, "Unsuccessful Remit", "Your remit application was rejected", "en-US");
            }

            return new MessageModel<string>()
            {
                Data = "transaction status update"
            };
        }

        /// <summary>
        /// 此支API是模擬使用者繳款後(only for demo)
        /// </summary>
        [HttpPost("SimulatingUserPay")]
        public MessageModel<string> SimulatingUserPay(TransactionStatusModifyRequest request)
        {
            
            statusModifier.SimulateingPaying(request.RecordId, request.Comment);
            
            return new MessageModel<string>()
            {
                Msg = "simulating paying successful"
            };
        }

        [HttpGet("")]
        public ActionResult<MessageModel<List<RemitRecordAdminDTO>>> GetRemitList()
        {
            List<RemitRecord> records = remitRecordService.GetAllRemitRecords();
            List<RemitRecordAdminDTO> remitRecordDTOs = _mapper.Map<List<RemitRecordAdminDTO>>(records);
            return Ok(new MessageModel<List<RemitRecordAdminDTO>>
            {
                Data = remitRecordDTOs
            });
        }

        [HttpGet("filter")]
        public ActionResult<MessageModel<List<RemitRecordAdminDTO>>> GetRemitFilterList(RemitFilterListRequest request)
        {
            List<RemitRecord> records = remitRecordService.GetRemitFilterRecords(request);
            List<RemitRecordAdminDTO> remitRecordDTOs = _mapper.Map<List<RemitRecordAdminDTO>>(records);
            return Ok(new MessageModel<List<RemitRecordAdminDTO>>
            {
                Data = remitRecordDTOs
            });
        }

        [HttpGet("{id}")]
        public ActionResult<MessageModel<RemitRecordAdminDTO>> GetRemitListById([FromRoute, SwaggerParameter("交易紀錄id", Required = true)] long id)
        {
            RemitRecord record = remitRecordService.GetRemitRecordById(id);
            ReceiveBank bank = bankService.GetReceiveBanks().Find(bank=>bank.Id==record.Beneficiary.ReceiveBankId);
            RemitRecordAdminDTO remitRecordAdminDTO = _mapper.Map<RemitRecordAdminDTO>(record);
            remitRecordAdminDTO.Bank = bank.TwName;
            return Ok(new MessageModel<RemitRecordAdminDTO>
            {
                Data = remitRecordAdminDTO
            });
        }

        [HttpGet("/image/remit/{remitId}/{type}")]
        public async Task<IActionResult> GetUserImage([FromRoute, SwaggerParameter("匯款申請單id", Required = true)] long remitId,
            [FromRoute, SwaggerParameter("0:及時照,1:簽名照", Required = true)] byte type)
        {
            RemitRecord record = remitRecordService.GetRemitRecordById(remitId);
            string imageFileName = null;
            switch (type)
            {
                case (byte)PictureTypeEnum.Instant:
                    imageFileName = record.RealTimePic;
                    break;
                case (byte)PictureTypeEnum.Signature:
                    imageFileName = record.ESignature;
                    break;
            }

            var image = await uploadPicService.LoadImage(null, type, imageFileName);
            if (image == null) return NotFound();

            return File(image, "image/jpeg");
        }
    }
}
