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

namespace Richviet.Admin.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("admin/remit/v{version:apiVersion}/review")]
    [ApiController]
    public class RemitReviewController : ControllerBase
    {

        private readonly RemitRecordAmlReviewer amlReviewer;
        private readonly RemitTransactionStatusModifier statusModifier;
        private readonly IRemitRecordService remitRecordService;
        private readonly IMapper _mapper;

        public RemitReviewController(RemitRecordAmlReviewer amlReviewer, RemitTransactionStatusModifier statusModifier, IRemitRecordService remitRecordService, IMapper mapper)
        {
            this.amlReviewer = amlReviewer;
            this.statusModifier = statusModifier;
            this.remitRecordService = remitRecordService;
            this._mapper = mapper;
        }


        [HttpPost("AmlReview")]
        public MessageModel<string> AmlReview(AmlReviewModifyRequest request)
        {

            if (request.IsAmlPass)
            {
                amlReviewer.AmlReviewPass(request.RecordId, request.Comment);
            }
            else
            {
                amlReviewer.AmlReviewFail(request.RecordId, request.Comment);
            }

            return new MessageModel<string>()
            {
                Data = "Aml status update"
            };
        }

        [HttpPost("Complete")]
        public MessageModel<string> TransSuccess(TransactionStatusModifyRequest request)
        {
            if (request.IsComplete)
            {
                statusModifier.RemitSuccess(request.RecordId, request.Comment);
            }
            else
            {
                statusModifier.RemitFail(request.RecordId, request.Comment);
            }

            return new MessageModel<string>()
            {
                Data = "transaction status update"
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
        [HttpGet("{id}")]
        public ActionResult<MessageModel<RemitRecordAdminDTO>> GetRemitListById([FromRoute, SwaggerParameter("交易紀錄id", Required = true)] long id)
        {
            RemitRecord record = remitRecordService.GetRemitRecordById(id);
            return Ok(new MessageModel<RemitRecordAdminDTO>
            {
                Data = _mapper.Map<RemitRecordAdminDTO>(record)
        });
        }
    }
}
