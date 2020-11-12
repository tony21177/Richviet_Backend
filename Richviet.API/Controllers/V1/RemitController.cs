using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic.CompilerServices;
using Richviet.API.DataContracts.Dto;
using Richviet.API.DataContracts.Requests;
using Richviet.API.DataContracts.Responses;
using Richviet.Services.Constants;
using Richviet.Services.Contracts;
using Richviet.Services.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Net;
using System.Security.Claims;

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

        private readonly IBeneficiarService beneficiarService;

        private readonly IDiscountService discountService;

        private readonly ICurrencyService currencyService;

        private readonly IUploadPic uploadPicService;

        public RemitController(ILogger<RemitController> logger, IMapper mapper, IRemitSettingService remitSettingService, IRemitRecordService remitRecordService, IBeneficiarService beneficiarService
            , IUserService userService, IDiscountService discountService, ICurrencyService currencyService, IUploadPic uploadPicService)
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._mapper = mapper;
            this.userService = userService;
            this.remitSettingService = remitSettingService;
            this.beneficiarService = beneficiarService;
            this.discountService = discountService;
            this.uploadPicService = uploadPicService;
            this.currencyService = currencyService;
            this.remitRecordService = remitRecordService;
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
            var userId = int.Parse(User.FindFirstValue("id"));
            UserArc userArc = userService.GetUserArcById(userId);
            if (!CheckIfKYCPassed(userArc))
                return BadRequest(new MessageModel<RemitRecordDTO>
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Success = false,
                    Msg = "KYC process Has not been passed"
                });
            RemitRecord record = remitRecordService.GetOngoingRemitRecordByUserId(userId);
            if (record == null)
            {
                record = remitRecordService.CreateRemitRecordByUserId(userId,PayeeTypeEnum.Bank);
                return Ok(new MessageModel<RemitRecordDTO>
                {
                    Data = _mapper.Map<RemitRecordDTO>(record)

                });
            }
            else if (record.TransactionStatus != (byte)RemitTransactionStatusEnum.Draft)
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
            var userId = int.Parse(User.FindFirstValue("id"));
            UserArc userArc = userService.GetUserArcById(userId);
            if (!CheckIfKYCPassed(userArc))
                return BadRequest(new MessageModel<RemitRecordDTO>
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Success = false,
                    Msg = "KYC process Has not been passed"
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
            string error = CheckAndSetDraftRemitProperty(userArc,record,draftRemitRequest, draftRemitRequest.Country??"TW");
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
        [HttpPost]
        [Authorize]
        public ActionResult<MessageModel<RemitRecordDTO>> ApplyRemitRecord([FromBody] RemitRequest remitRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            // KYC passed?
            var userId = int.Parse(User.FindFirstValue("id"));
            UserArc userArc = userService.GetUserArcById(userId);
            if(!CheckIfKYCPassed(userArc))
                return BadRequest(new MessageModel<RemitRecordDTO>
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Success = false,
                    Msg = "KYC process Has not been passed"

                });
            string error = null;
            // check amount
            error = CheckIfAmountOutOfRange(remitRequest.FromAmount, "TW");
            if (error!=null) 
                return BadRequest(new MessageModel<RemitRecordDTO>
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Success = false,
                    Msg = error

                });
            // check beneficiar
            error = CheckBenificiarExistence(remitRequest.BeneficiarId);
            if (error != null)
            {
                return BadRequest(new MessageModel<RemitRecordDTO>
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Success = false,
                    Msg = error

                });
            }

            // check uploaded picture
            error = CheckPhotoFileExistence(userArc, remitRequest.PhotoFilename);
            if (error != null)
            {
                return BadRequest(new MessageModel<RemitRecordDTO>
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Success = false,
                    Msg = error

                });
            }
            error = CheckSignatureFileExistence(userArc, remitRequest.SignatureFilename);
            if (error != null)
            {
                return BadRequest(new MessageModel<RemitRecordDTO>
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Success = false,
                    Msg = error

                });
            }
            // TBD

            return Ok(new MessageModel<RemitRecordDTO>
            {
                Data = new RemitRecordDTO() {
                    Id = 101,
                    CreateTime = DateTimeOffset.Now.ToUnixTimeSeconds(),
                    ToCurrency = "VND",
                    FromAmount = 10000,
                    ToAmount = 7960000,
                    TransactionExchangeRate = 800,
                    Fee = 150,
                    DiscountAmount = 100,
                    PayeeName = "爸爸",
                    PayeeAddress = "XXXXXXXXXX456",
                    PayeeRelationType = 0,
                    PayeeRelationTypeDescription = "父母",
                    TransactionStatus = 0
                }
            });
        }

        /// <summary>
        /// 取得該筆匯款紀錄的繳款碼
        /// </summary>
        /// 
        [HttpGet("payment/{id}")]
        [AllowAnonymous]
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
        [AllowAnonymous]
        public ActionResult<MessageModel<RemitRecordDTO []>> GetRemitRecords()
        {
            return Ok(new MessageModel<RemitRecordDTO[]>
            {
                Data =  new RemitRecordDTO[] {
                    new RemitRecordDTO()
                    {
                        Id = 101,
                        CreateTime = DateTimeOffset.Now.ToUnixTimeSeconds(),
                        ToCurrency = "VND",
                        FromAmount = 10000,
                        ToAmount = 7960000,
                        TransactionExchangeRate = 800,
                        Fee = 150,
                        DiscountAmount = 100,
                        PayeeName = "爸爸",
                        PayeeAddress = "XXXXXXXXXX456",
                        PayeeRelationType = 0,
                        TransactionStatus = 0
                    }, new RemitRecordDTO()
                    {
                        Id = 102,
                        CreateTime = DateTimeOffset.Now.ToUnixTimeSeconds(),
                        ToCurrency = "USD",
                        FromAmount = 10000,
                        ToAmount = 348.25,
                        TransactionExchangeRate = 0.035,
                        Fee = 150,
                        DiscountAmount = 100,
                        PayeeName = "爸爸",
                        PayeeAddress = "XXXXXXXXXX456",
                        PayeeRelationType = 0,
                        TransactionStatus = 1
                    }
                }
            });
        }


        private string CheckIfAmountOutOfRange(int amount,string country)
        {
            BussinessUnitRemitSetting remitSetting = remitSettingService.GetRemitSettingByCountry(country);
            if (remitSetting == null) return "no remit setting for {country}";
            if (amount < remitSetting.RemitMin || amount > remitSetting.RemitMax) return "amount out of range";
            return null;
        }

        private string CheckBenificiarExistence(int id)
        {
            OftenBeneficiar beneficiar = beneficiarService.GetBeneficiarById(id);
            if (beneficiar == null)
            {
                return "beneficiar does not exist!";
            }
            return null;
        }

        private string CheckDiscountExistence(int id)
        {
            Discount discount = discountService.GetDoscountById(id);
            if (discount == null)
            {
                return "discount does not exist!";
            }
            return null;
        }

        private string CheckCurrencyExistence(int id)
        {
            CurrencyCode currency = currencyService.GetCurrencyById(id);
            if (currency == null)
            {
                return "currency does not exist!";
            }
            return null;
        }

        private string CheckPhotoFileExistence(UserArc userArc,string filename)
        {
            if (!uploadPicService.CheckUploadFileExistence(userArc, PictureTypeEnum.Instant, filename))
                return "Photo file does not exist!";
            return null;
        }

        private string CheckSignatureFileExistence(UserArc userArc, string filename)
        {
            if (!uploadPicService.CheckUploadFileExistence(userArc, PictureTypeEnum.Signature, filename))
                return "Signature file does not exist!";
            return null;
        }

        private bool CheckIfKYCPassed(UserArc userArc)
        {

            if (userArc != null && userArc.KycStatus == (byte)KycStatusEnum.PASSED_KYC)
            {
                return true;
            }
            return false;
        }

        private string CheckAndSetDraftRemitProperty(UserArc userArc,RemitRecord remitRecord,DraftRemitRequest draftRemitRequest,String country)
        {
            string error = null;
            if (draftRemitRequest.FromAmount != null)
            {
                error = CheckIfAmountOutOfRange((int)draftRemitRequest.FromAmount, country);
                if (error != null) return error;
                remitRecord.FromAmount = (double)draftRemitRequest.FromAmount;
            }
   
            if (draftRemitRequest.BeneficiarId != null)
            {
                error = CheckBenificiarExistence((int)draftRemitRequest.BeneficiarId);
                if (error != null) return error;
                remitRecord.BeneficiarId = draftRemitRequest.BeneficiarId;
            }
                
            if (draftRemitRequest.PhotoFilename != null)
            {
                error = CheckPhotoFileExistence(userArc, (string)draftRemitRequest.PhotoFilename);
                if (error != null) return error;
                remitRecord.RealTimePic = draftRemitRequest.PhotoFilename;
            }
                
            if (draftRemitRequest.SignatureFilename != null)
            {
                error = CheckSignatureFileExistence(userArc,(string)draftRemitRequest.SignatureFilename);
                if (error != null) return error;
                remitRecord.ESignature = draftRemitRequest.SignatureFilename;
            }
                
            if (draftRemitRequest.DiscountId != null)
            {
                error = CheckDiscountExistence((int)draftRemitRequest.DiscountId);
                if (error != null) return error;
                remitRecord.DiscountId = draftRemitRequest.DiscountId;
            }
                
            if (draftRemitRequest.ToCurrencyId != null)
            {
                error = CheckCurrencyExistence((int)draftRemitRequest.ToCurrencyId);
                if (error != null) return error;
                remitRecord.ToCurrencyId = draftRemitRequest.ToCurrencyId;
            }
            return null;
        }

        


    }
}
