using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Richviet.Services.Constants;
using Richviet.Services.Contracts;
using Frontend.DB.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RemitRecords.Domains.RemitRecords.Constants;
using System.Threading.Tasks;
using Richviet.BackgroudTask.Arc.Vo;
using Richviet.BackgroudTask.Arc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Email.Notifier;

namespace Richviet.Services.Services
{
    public class RemitRecordService : IRemitRecordService
    {

        private readonly ILogger logger;
        private readonly GeneralContext dbContext;
        private readonly UserService userService;
        private readonly IArcScanRecordService arcScanRecordService;
        private readonly ArcValidationTask arcValidationTask;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly string workingRootPath;
        private readonly IConfiguration configuration;
        private readonly IEmailSender emailSender;
        private readonly string REMIT_ARC_PASSED_SUBJECT = "匯款流程-通過arc自動審核";
        private readonly string REMIT_ARC_PASSED_MESSAGE = "未通過arc自動審核";
        private readonly string REMIT_ARC_NOT_PASSED_SUBJECT = "匯款流程-未通過arc自動審核";
        private readonly string REMIT_ARC_NOT_PASSED_MESSAGE = "未通過arc自動審核";
        private readonly string[] receivers;

        public RemitRecordService(ILogger<RemitRecordService> logger, GeneralContext dbContext, UserService userService, IArcScanRecordService arcScanRecordService,
            IWebHostEnvironment webHostEnvironment, IConfiguration configuration, IEmailSender emailSender,ArcValidationTask arcValidationTask)
        {
            this.logger = logger;
            this.dbContext = dbContext;
            this.userService = userService;
            this.arcScanRecordService = arcScanRecordService;
            this.arcValidationTask = arcValidationTask;
            this.workingRootPath = webHostEnvironment.ContentRootPath;
            this.configuration = configuration;
            this.emailSender = emailSender;
            receivers = configuration.GetSection("ArcResultNotify").Get<string[]>();

        }


        public RemitRecord CreateRemitRecordByUserArc(UserArc userArc,RemitRecord remitRecord, PayeeTypeEnum payeeTypeEnum)
        {
            remitRecord.UserId = userArc.UserId;
            remitRecord.ArcName = userArc.ArcName;
            remitRecord.ArcNo = userArc.ArcNo;
            remitRecord.PayeeType = (byte)payeeTypeEnum;
            
            dbContext.RemitRecord.Add(remitRecord);
            dbContext.SaveChanges();
            dbContext.Entry(remitRecord).Reference(record => record.Beneficiary).Query()
                .Include(Beneficiary => Beneficiary.PayeeRelation)
                .Load();
            dbContext.Entry(remitRecord).Reference(record => record.ToCurrency).Load();

            return remitRecord;
        }

        public List<RemitRecord> GetOngoingRemitRecordsByUserArc(UserArc userArc)
        {
            short[] completedStatus = 
            {
                (short)RemitTransactionStatusEnum.Complete,(short)RemitTransactionStatusEnum.FailedVerified,(short)RemitTransactionStatusEnum.OtherError,
            };
            List<short> completedStatusList = completedStatus.ToList();
            return dbContext.RemitRecord.Where<RemitRecord>(record => record.UserId== userArc.UserId && !completedStatusList.Contains(record.TransactionStatus)).ToList();
        }



        public RemitRecord GetDraftRemitRecordByUserArc(UserArc userArc)
        {
            List<RemitRecord> onGogingRemitRecords = GetOngoingRemitRecordsByUserArc(userArc);
            return onGogingRemitRecords.Find(record => record.TransactionStatus == (short)RemitTransactionStatusEnum.Draft);
        }


        public RemitRecord GetRemitRecordById(long id)
        {
            RemitRecord record = dbContext.RemitRecord.Find(id);
            if (record != null)
            {
                dbContext.Entry(record).Reference(record => record.Beneficiary).Query()
                .Include(Beneficiary => Beneficiary.PayeeRelation)
                .Load();
                dbContext.Entry(record).Reference(record => record.ToCurrency).Load();
            }
            return record;
        }

        public RemitRecord ModifyRemitRecord(RemitRecord modifiedRemitRecord,DateTime? applyTime)
        {
            if (applyTime != null)
            {
                modifiedRemitRecord.FormalApplyTime = applyTime;
            }

            modifiedRemitRecord.UpdateTime = DateTime.UtcNow;
            dbContext.RemitRecord.Update(modifiedRemitRecord);
            dbContext.SaveChanges();
            dbContext.Entry(modifiedRemitRecord).Reference(record => record.Beneficiary).Query()
            .Include(Beneficiary => Beneficiary.PayeeRelation)
            .Load();
            dbContext.Entry(modifiedRemitRecord).Reference(record => record.ToCurrency).Load();
            return modifiedRemitRecord;
        }

        public List<RemitRecord> GetRemitRecordsByUserId(long userId)
        {
            return dbContext.RemitRecord.Include(record=>record.Beneficiary).ThenInclude(Beneficiary=>Beneficiary.PayeeRelation).Include("ToCurrency").Where(record => record.UserId == userId).ToList();
        }

        public List<string> GeneratePaymentCode(RemitRecord modifiedRemitRecord)
        {
            List<string> codeList = new List<string>();
            codeList.Add("100302C72");
            codeList.Add("1231231231000000");
            codeList.Add("090673000020000");
            string codeStr = String.Join(",", codeList.ToArray());
            modifiedRemitRecord.UpdateTime = DateTime.UtcNow;
            modifiedRemitRecord.PaymentCode = codeStr;
            dbContext.RemitRecord.Update(modifiedRemitRecord);
            dbContext.SaveChanges();

            return codeList;
        }



        public void DeleteRmitRecord(RemitRecord record)
        {
            dbContext.RemitRecord.Remove(record);
            dbContext.SaveChanges();
        }

        public async Task SystemVerifyArcForRemitProcess(RemitRecord remitRecord, long userId)
        {
            UserArc userArc = userService.GetUserArcById(userId);
            if (userArc.ArcIssueDate == null || userArc.ArcExpireDate == null)
            {
                throw new Exception("ARC Data not sufficient");
            }
            ArcValidationResult arcValidationResult = arcValidationTask.Validate(workingRootPath, userArc.ArcNo, ((DateTime)userArc.ArcIssueDate).ToString("yyyyMMdd"), ((DateTime)userArc.ArcExpireDate).ToString("yyyyMMdd"), userArc.BackSequence).Result;
            if (arcValidationResult.IsSuccessful)
            {
                ArcScanRecord record = new ArcScanRecord()
                {
                    ArcStatus = (short)SystemArcVerifyStatusEnum.PASS,
                    ScanTime = DateTime.UtcNow,
                    Description = arcValidationResult.Result,
                    Event = (byte)ArcScanEvent.Remit
                };

                remitRecord.TransactionStatus = (short)RemitTransactionStatusEnum.SuccessfulArcVerification;
                AddScanRecordAndUpdateUserKycStatus(record, userArc, remitRecord);
                // send mail
                await SendMailForRemitArc(true, receivers, userId);
            }
            else
            {
                ArcScanRecord record = new ArcScanRecord()
                {
                    ArcStatus = (short)SystemArcVerifyStatusEnum.FAIL,
                    ScanTime = DateTime.UtcNow,
                    Description = arcValidationResult.Result,
                    Event = (byte)ArcScanEvent.Remit
                };
                remitRecord.TransactionStatus = (short)RemitTransactionStatusEnum.FailedVerified;
                userArc.KycStatus = (short)KycStatusEnum.FAILED_KYC;
                userArc.KycStatusUpdateTime = DateTime.UtcNow;
                AddScanRecordAndUpdateUserKycStatus(record, userArc, remitRecord);
                // send mail
                await SendMailForRemitArc(false, receivers, userId);
            }
        }


        private void AddScanRecordAndUpdateUserKycStatus(ArcScanRecord record, UserArc userArc, RemitRecord remitRecord)
        {
            using var transaction = dbContext.Database.BeginTransaction();
            try
            {

                // for demo
                if (configuration["IsDemo"] != null && bool.Parse(configuration["IsDemo"]) == true)
                {
                    var demoArcArray = configuration.GetSection("DemoArc").Get<string[]>();
                    if (Array.IndexOf(demoArcArray, userArc.ArcNo) > -1)
                    {
                        remitRecord.TransactionStatus = (short)RemitTransactionStatusEnum.SuccessfulAmlVerification;
                        userArc.KycStatus = (short)KycStatusEnum.PASSED_KYC_FORMAL_MEMBER;
                    }
                }
                //
                arcScanRecordService.AddScanRecord(record);
                userArc.LastArcScanRecordId = record.Id;
                userArc.UpdateTime = DateTime.UtcNow;
                dbContext.UserArc.Update(userArc);
                dbContext.SaveChanges();
                remitRecord.ArcScanRecordId = record.Id;
                dbContext.RemitRecord.Update(remitRecord);
                dbContext.SaveChanges();
                transaction.Commit();
                return;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, null);
                transaction.Rollback();
            }
        }

        private async Task SendMailForRemitArc(bool isSuccessful, string[] mails, long userId)
        {
            logger.LogInformation("Send mail.......");
            SendEmailVo emailVo = new SendEmailVo();
            if (isSuccessful)
            {
                emailVo.Subject = REMIT_ARC_PASSED_SUBJECT;
                emailVo.Message = $"使用者id:{userId}" + REMIT_ARC_PASSED_MESSAGE;
            }
            else
            {
                emailVo.Subject = REMIT_ARC_NOT_PASSED_SUBJECT;
                emailVo.Message = $"{userId}" + REMIT_ARC_NOT_PASSED_MESSAGE;
            }
            foreach (var mail in mails)
            {
                emailVo.Email = mail;
                await emailSender.SendEmailAsync(emailVo);
            }
        }
    }
}
