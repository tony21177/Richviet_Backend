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


        public RemitController(ILogger<RemitController> logger, IMapper mapper, IRemitSettingService remitSettingService, IRemitRecordService remitRecordService, IBeneficiarService beneficiarService
            , IUserService userService, IDiscountService discountService, RemitValidationHelper helper)
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._mapper = mapper;
            this.userService = userService;
            this.remitSettingService = remitSettingService;
            this.discountService = discountService;
            this.remitRecordService = remitRecordService;
            this.helper = helper;
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
        /// 申請匯款草稿
        /// </summary>
        [HttpPost("draft")]
        [Authorize]
        public ActionResult<MessageModel<RemitRecordDTO>> CreateDraftRemitRecord()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
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
            RemitRecord record = remitRecordService.GetOngoingRemitRecordByUserArc(userArc);
            if (record == null)
            {
                record = remitRecordService.CreateRemitRecordByUserArc(userArc, PayeeTypeEnum.Bank);
                return Ok(new MessageModel<RemitRecordDTO>
                {
                    Data = _mapper.Map<RemitRecordDTO>(record)

                });
            }
            else if (record.TransactionStatus != (short)RemitTransactionStatusEnum.Draft)
            {
                return BadRequest(new MessageModel<RemitRecordDTO>
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Success = false,
                    Msg = "your remit application is under processing"
                });
            }
            else
            {
                return BadRequest(new MessageModel<RemitRecordDTO>
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Success = false,
                    Msg = "the draft already exists"
                });
            }
        }


        /// <summary>
        /// 修改匯款草稿(HTTP PATCH method)
        /// </summary>
        [HttpPatch("draft/{id}")]
        [Authorize]
        public ActionResult<MessageModel<RemitRecordDTO>> PatchDraftRemitRecord([FromRoute, SwaggerParameter("草稿匯款的id", Required = true)] int id, [FromBody] DraftRemitRequest draftRemitRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
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
            // check existence
            RemitRecord record = remitRecordService.GetRemitRecordById(id);
            if (record == null)
            {
                return BadRequest(new MessageModel<RemitRecordDTO>
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Success = false,
                    Msg = "Not exists"
                });
            }
            string error = helper.CheckAndSetDraftRemitProperty(userArc,record,draftRemitRequest, draftRemitRequest.Country??"TW");
            if (error != null)
            {
                return BadRequest(new MessageModel<RemitRecordDTO>
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Success = false,
                    Msg = error
                });
            }
            else
            {
                remitRecordService.ModifyRemitRecord(record);
                RemitRecordDTO remitRecordDTO = _mapper.Map<RemitRecordDTO>(record);
                return Ok(new MessageModel<RemitRecordDTO>
                {
                    Data = remitRecordDTO
                });
            }
        }

        /// <summary>
        /// 使用者送出正式匯款申請
        /// </summary>
        [HttpPost("formal/{id}")]
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
            
            RemitRecord modifiedRecord = remitRecordService.ModifyRemitRecord(record);
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
        public ActionResult<MessageModel<PaymentCodeDTO>> GetPaymentCode([FromRoute, SwaggerParameter("交易紀錄id", Required = true)] int id)
        {
            return Ok(new MessageModel<PaymentCodeDTO>()
            {
                Data = new PaymentCodeDTO()
                {
                    Code = "WEFQEWFEFQEQGRGRG009233"
                }
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
