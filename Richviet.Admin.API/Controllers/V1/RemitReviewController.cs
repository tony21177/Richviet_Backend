﻿using Microsoft.AspNetCore.Mvc;
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

namespace Richviet.Admin.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("admin/remit/v{version:apiVersion}/review")]
    [ApiController]
    public class RemitReviewController : ControllerBase
    {

        private readonly RemitRecordAmlReviewer amlReviewer;
        private readonly RemitTransactionStatusModifier statusModifier;

        public RemitReviewController(RemitRecordAmlReviewer amlReviewer, RemitTransactionStatusModifier statusModifier)
        {
            this.amlReviewer = amlReviewer;
            this.statusModifier = statusModifier;
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
    }
}
