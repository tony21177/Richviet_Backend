using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Richviet.API.DataContracts.Dto;
using Richviet.API.DataContracts.Requests;
using Richviet.API.DataContracts.Responses;
using Richviet.Services.Constants;
using Richviet.Services.Contracts;
using Frontend.DB.EF.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Net;
using System.Security.Claims;
using Hangfire;
using Richviet.API.Helper;
using System.Collections.Generic;
using System.Linq;
using RemitRecords.Domains.RemitRecords.Constants;
using RemitRecords.Domains.RemitRecords.Query;
using RemitRecords.Domains.RemitRecords.Vo;

#pragma warning disable 1591
namespace Richviet.API.Controllers.V1
{

    /// <summary>
    /// 匯款流程相關API
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/remit")]
    [ApiController]
    public class RemitController : Controller
    {
        private readonly ILogger Logger;

        private readonly IMapper _mapper;

        private readonly IUserService userService;

        private readonly IRemitSettingService remitSettingService;

        private readonly IRemitRecordService remitRecordService;

        private readonly IDiscountService discountService;

        private readonly RemitValidationHelper helper;

        private readonly string KYC_NOT_PASSED = "KYC process has not been passed or failed!";

        private readonly string NOT_AUTHORIZED = "Unauthorized Operation";

        private readonly string COUNTRY = "TW";

        private readonly IRemitRecordQueryRepositories remitRecordQueryRepositories;


        public RemitController(ILogger<RemitController> logger, IMapper mapper, IRemitSettingService remitSettingService, IRemitRecordService remitRecordService, IBeneficiarService beneficiarService
            , IUserService userService, IDiscountService discountService, IRemitRecordQueryRepositories remitRecordQueryRepositories, RemitValidationHelper helper)
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._mapper = mapper;
            this.userService = userService;
            this.remitSettingService = remitSettingService;
            this.discountService = discountService;
            this.remitRecordService = remitRecordService;
            this.helper = helper;
            this.remitRecordQueryRepositories = remitRecordQueryRepositories;
        }

        /// <summary>
        /// 取得服務所在國家的匯款相關設定(min,max)
        /// </summary>
        [HttpGet("settings/{country}")]
        [Authorize]
        public MessageModel<RemitSettingDTO> GetCurrencyInfo([FromRoute, SwaggerParameter("國家 e.g. TW ", Required = true)] string country)
        {
            Logger.LogInformation(country);
            BussinessUnitRemitSetting setting = remitSettingService.GetRemitSettingByCountry(country.ToUpper());
            RemitSettingDTO settingDTO = _mapper.Map<RemitSettingDTO>(setting);

            return new MessageModel<RemitSettingDTO>
            {
                Data = settingDTO
            };

        }

        /// <summary>
        /// 使用者擁有的優惠券
        /// </summary>
        [HttpGet("discount")]
        [AllowAnonymous]
        public MessageModel<UserRemitDiscountDTO[]> GetUserDiscount()
        {

            return new MessageModel<UserRemitDiscountDTO[]>
            {
                Data = new UserRemitDiscountDTO[2]
                {
                    new UserRemitDiscountDTO()
                    {
                        Id=2,
                        UserId=10,
                        EffectiveDate=new DateTime(2020,11,1),
                        ExpireDate=new DateTime(2021,3,31),
                        Value=50
                    },
                    new UserRemitDiscountDTO()
                    {
                        Id=3,
                        UserId=10,
                        EffectiveDate=new DateTime(2020,11,1),
                        ExpireDate=new DateTime(2021,3,31),
                        Value=100
                    }
                }
            };

        }

        /// <summary>
        /// 使用者可匯款的月額度,年額度
        /// </summary>
        [HttpGet("availableAmount")]
        [Authorize]
        public MessageModel<RemitAvailableAmount> GetAvailableAmount()
        {
            var userId = long.Parse(User.FindFirstValue("id"));
            RemitAvailableAmountSumVo amountSumVo = remitRecordQueryRepositories.QueryRemitAvailableAmount(userId, COUNTRY);


            return new MessageModel<RemitAvailableAmount>
            {
                Data = new RemitAvailableAmount
                {
                    MonthlyAvailableRemitAmount = (int) amountSumVo.MonthlyAvailableRemitAmount,
                    YearlyAvailableRemitAmount =  (int) amountSumVo.YearlyAvailableRemitAmount
                }
            };

        }

        ///// <summary>
        ///// 取得使用者目前的草稿,和剩餘可匯款月限額跟年限額(不包括此筆草稿的金額)
        ///// </summary>
        //[HttpGet("draft")]
        //[Authorize]
        //public ActionResult<MessageModel<DraftRemitDTO>> GetDraftRemitRecord()
        //{
        //    // KYC passed?
        //    var userId = long.Parse(User.FindFirstValue("id"));
        //    UserArc userArc = userService.GetUserArcById(userId);
        //    if (!helper.CheckIfKYCPassed(userArc))
        //        return BadRequest(new MessageModel<DraftRemitDTO>
        //        {
        //            Status = (int)HttpStatusCode.BadRequest,
        //            Success = false,
        //            Msg = KYC_NOT_PASSED
        //        });
        //    RemitRecord draftRemitRecord = remitRecordService.GetDraftRemitRecordByUserArc(userArc);
        //    RemitAvailableAmountSumVo amountSumVo = remitRecordQueryRepositories.QueryRemitAvailableAmount(userId, COUNTRY);
        //    DraftRemitDTO data = new DraftRemitDTO()
        //    {
        //        RemitRecordDTO = _mapper.Map<RemitRecordDTO>(draftRemitRecord),
        //        MonthlyAvailableRemitAmount = (int)amountSumVo.MonthlyAvailableRemitAmount,
        //        YearlyAvailableRemitAmount = (int)amountSumVo.YearlyAvailableRemitAmount
        //    };


        //    return Ok(new MessageModel<DraftRemitDTO>
        //    {
        //        Data = data
        //    });
        //}


        ///// <summary>
        ///// 刪除使用者目前的草稿
        ///// </summary>
        //[HttpDelete("draft")]
        //[Authorize]
        //public ActionResult<MessageModel<RemitRecordDTO>> RemoveDraftRemitRecord()
        //{
        //    // KYC passed?
        //    var userId = long.Parse(User.FindFirstValue("id"));
        //    UserArc userArc = userService.GetUserArcById(userId);
        //    if (!helper.CheckIfKYCPassed(userArc))
        //        return BadRequest(new MessageModel<RemitRecordDTO>
        //        {
        //            Status = (int)HttpStatusCode.BadRequest,
        //            Success = false,
        //            Msg = KYC_NOT_PASSED
        //        });
        //    RemitRecord draftRemitRecord = remitRecordService.GetDraftRemitRecordByUserArc(userArc);
        //    if(draftRemitRecord == null)
        //    {
        //        return BadRequest(new MessageModel<RemitRecordDTO>
        //        {
        //            Status = (int)HttpStatusCode.BadRequest,
        //            Success = false,
        //            Msg = "draft remit record does not exist!"
        //        }); ;
        //    }
        //    remitRecordService.DeleteRmitRecord(draftRemitRecord);
        //    return Ok(new MessageModel<RemitRecordDTO>
        //    {
        //        Data = null
        //    }); ;
        //}

        /// <summary>
        /// 申請匯款草稿
        /// </summary>
        [HttpPost("draft")]
        [Authorize]
        public ActionResult<MessageModel<RemitRecordDTO>> CreateDraftRemitRecord([FromBody] DraftRemitRequest draftRemitRequest)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();

                return BadRequest(new MessageModel<RemitRecordDTO>
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Success = false,
                    Msg = string.Join(",", errors)
                });
            }
            // KYC passed?
            var userId = long.Parse(User.FindFirstValue("id"));
            UserArc userArc = userService.GetUserArcById(userId);
            if (!helper.CheckIfKYCPassed(userArc))
                return BadRequest(new MessageModel<RemitRecordDTO>
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Success = false,
                    Msg = KYC_NOT_PASSED
                });
            // Get draft
            RemitRecord draftRemitRecord = remitRecordService.GetDraftRemitRecordByUserArc(userArc);
            if (draftRemitRecord != null)
            {
                remitRecordService.DeleteRmitRecord(draftRemitRecord);
            }

            draftRemitRecord = new RemitRecord();
            string error = helper.CheckAndSetDraftRemitProperty(userArc, draftRemitRecord, draftRemitRequest, draftRemitRequest.Country ?? "TW");
            if (error != null)
            {
                return BadRequest(new MessageModel<RemitRecordDTO>
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Success = false,
                    Msg = error
                });
            }
            draftRemitRecord = remitRecordService.CreateRemitRecordByUserArc(userArc, draftRemitRecord, PayeeTypeEnum.Bank);
                

            return Ok(new MessageModel<RemitRecordDTO>
            {
                Data = _mapper.Map<RemitRecordDTO>(draftRemitRecord)

            });
            
            
        }


        ///// <summary>
        ///// 修改匯款草稿(HTTP PATCH method)
        ///// </summary>
        //[HttpPatch("draft/{id}")]
        //[Authorize]
        //public ActionResult<MessageModel<RemitRecordDTO>> PatchDraftRemitRecord([FromRoute, SwaggerParameter("草稿匯款的id", Required = true)] int id, [FromBody] DraftRemitRequest draftRemitRequest)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest();
        //    }
        //    // KYC passed?
        //    var userId = long.Parse(User.FindFirstValue("id"));
        //    UserArc userArc = userService.GetUserArcById(userId);
        //    if (!helper.CheckIfKYCPassed(userArc))
        //        return BadRequest(new MessageModel<RemitRecordDTO>
        //        {
        //            Status = (int)HttpStatusCode.BadRequest,
        //            Success = false,
        //            Msg = KYC_NOT_PASSED
        //        });
        //    // check existence
        //    RemitRecord record = remitRecordService.GetRemitRecordById(id);
        //    if (record == null)
        //    {
        //        return BadRequest(new MessageModel<RemitRecordDTO>
        //        {
        //            Status = (int)HttpStatusCode.BadRequest,
        //            Success = false,
        //            Msg = "Not exists"
        //        });
        //    }
        //    string error = helper.CheckAndSetDraftRemitProperty(userArc,record,draftRemitRequest, draftRemitRequest.Country??"TW");
        //    if (error != null)
        //    {
        //        return BadRequest(new MessageModel<RemitRecordDTO>
        //        {
        //            Status = (int)HttpStatusCode.BadRequest,
        //            Success = false,
        //            Msg = error
        //        });
        //    }
        //    else
        //    {
        //        remitRecordService.ModifyRemitRecord(record,null);
        //        RemitRecordDTO remitRecordDTO = _mapper.Map<RemitRecordDTO>(record);
        //        return Ok(new MessageModel<RemitRecordDTO>
        //        {
        //            Data = remitRecordDTO
        //        });
        //    }
        //}

        /// <summary>
        /// 使用者送出正式匯款申請
        /// </summary>
        [HttpPost("formalRemit/{id}")]
        [Authorize]
        public ActionResult<MessageModel<RemitRecordDTO>> ApplyRemitRecord([FromBody] RemitRequest remitRequest, [FromRoute, SwaggerParameter("草稿匯款的id", Required = true)] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            
            // KYC passed?
            var userId = long.Parse(User.FindFirstValue("id"));
            UserArc userArc = userService.GetUserArcById(userId);
            if(!helper.CheckIfKYCPassed(userArc))
                return BadRequest(new MessageModel<RemitRecordDTO>
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Success = false,
                    Msg = KYC_NOT_PASSED

                });

            // get draft remit
            RemitRecord record = remitRecordService.GetRemitRecordById(id);
            if(record==null || record.TransactionStatus != (short)RemitTransactionStatusEnum.Draft)
            {
                return BadRequest(new MessageModel<RemitRecordDTO>
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Success = false,
                    Msg = "This Draft Remit doesn't exit or wrong status"

                });
            }

            string validationResult = helper.ValidateFormalRemitRequestAndUpdateRemitRecord(remitRequest,record,userArc,remitRequest.Country??"TW");
            if (validationResult != null)
            {
                return BadRequest(new MessageModel<RemitRecordDTO>
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Success = false,
                    Msg = validationResult

                });
            }

            DateTime now = DateTime.UtcNow;
            RemitRecord modifiedRecord = remitRecordService.ModifyRemitRecord(record, now);
            RemitRecordDTO recordDTO = _mapper.Map<RemitRecordDTO>(modifiedRecord);

            // 系統掃ARC No.
            BackgroundJob.Enqueue(() => userService.SystemVerifyArcForRemitProcess(modifiedRecord, userId));

            return Ok(new MessageModel<RemitRecordDTO>
            {
                Data = recordDTO
            });
        }

        /// <summary>
        /// 取得該筆匯款紀錄的繳款碼
        /// </summary>
        /// 
        [HttpGet("payment/{id}")]
        [Authorize]
        public ActionResult<MessageModel<PaymentCodeDTO>> GetPaymentCode([FromRoute, SwaggerParameter("交易紀錄id", Required = true)] long id)
        {
            return Ok(new MessageModel<PaymentCodeDTO>()
            {
                Data = new PaymentCodeDTO()
                {
                   Code = new string [] { "100302C72", "1231231231000000", "090673000020000" }
                }
            });
        }


        /// <summary>
        /// 取得該筆匯款紀錄明細
        /// </summary>
        /// 
        [HttpGet("remitRecords/{id}")]
        [Authorize]
        public ActionResult<MessageModel<RemitRecordDTO>> GetRemitRecordById([FromRoute, SwaggerParameter("交易紀錄id", Required = true)] long id)
        {
            var userId = long.Parse(User.FindFirstValue("id"));
            UserArc userArc = userService.GetUserArcById(userId);
            RemitRecord record = remitRecordService.GetRemitRecordById(id);
            if(record!=null && record.UserId != userId)
            {
                return Unauthorized(new MessageModel<RemitRecordDTO>
                {
                    Status = (int)HttpStatusCode.Unauthorized,
                    Success = false,
                    Msg = NOT_AUTHORIZED
                });
            }


            return Ok(new MessageModel<RemitRecordDTO>()
            {
                Data = _mapper.Map<RemitRecordDTO>(record)
            });
        }

        /// <summary>
        /// 該會員的交易紀錄
        /// </summary>
        /// 
        [HttpGet("remitRecords")]
        [Authorize]
        public ActionResult<MessageModel<List<RemitRecordDTO>>> GetRemitRecords()
        {
            var userId = long.Parse(User.FindFirstValue("id"));
            List<RemitRecord> remitRecords = remitRecordService.GetRemitRecordsByUserId(userId);
            List<RemitRecordDTO> remitRecordDTOs = _mapper.Map<List<RemitRecordDTO>>(remitRecords);

            return Ok(new MessageModel<List<RemitRecordDTO>>
            {
                Data = remitRecordDTOs
            });
        }

    }
}
