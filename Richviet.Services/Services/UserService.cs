using Microsoft.EntityFrameworkCore;
using Richviet.Services.Constants;
using Richviet.Services.Contracts;
using Frontend.DB.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Richviet.Tools.Utility;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Richviet.BackgroudTask.Arc.Vo;
using Richviet.BackgroudTask.Arc;
using Microsoft.AspNetCore.Hosting;
using RemitRecords.Domains.RemitRecords.Constants;
using Microsoft.Extensions.Configuration;
using Email.Notifier;
using SendGrid;

namespace Richviet.Services
{
    public class UserService: IUserService
    {
        private readonly IEnumerable<IAuthService> authServices;
        private readonly IArcScanRecordService arcScanRecordService;
        private readonly IRemitRecordService remitRecordService;
        private readonly GeneralContext dbContext;
        private readonly ILogger logger;
        private readonly IMapper mapper;
        private readonly ArcValidationTask arcValidationTask;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly string workingRootPath;
        private readonly IConfiguration configuration;
        private readonly IEmailSender emailSender;
        private readonly string REGISTER_ARC_PASSED_SUBJECT = "註冊流程-通過arc自動審核";
        private readonly string REGISTER_ARC_PASSED_MESSAGE = "未通過arc自動審核";
        private readonly string REGISTER_ARC_NOT_PASSED_SUBJECT = "註冊流程-未通過arc自動審核";
        private readonly string REGISTER_ARC_NOT_PASSED_MESSAGE = "未通過arc自動審核";
        private readonly string[] receivers;

        public UserService(IEnumerable<IAuthService> authServices, IArcScanRecordService arcScanRecordService, IRemitRecordService remitRecordService,
            GeneralContext dbContext, ILogger<UserService> logger, IMapper mapper, ArcValidationTask arcValidationTask
            , IWebHostEnvironment webHostEnvironment, IConfiguration configuration, IEmailSender emailSender)
        {
            this.authServices = authServices;
            this.arcScanRecordService = arcScanRecordService;
            this.remitRecordService = remitRecordService;
            this.dbContext = dbContext;
            this.logger =  logger;
            this.mapper = mapper;
            this.arcValidationTask =  arcValidationTask;
            this.webHostEnvironment = webHostEnvironment;
            this.workingRootPath = webHostEnvironment.ContentRootPath;
            this.configuration = configuration;
            this.emailSender = emailSender;
            receivers = configuration.GetSection("ArcResultNotify").Get<string[]>();
        }

        public async Task<bool> AddNewUserInfo(UserRegisterType loginUser)
        {
            using var transaction = dbContext.Database.BeginTransaction();
            try
            {
                var user = new User();
                await dbContext.User.AddAsync(user);
                dbContext.SaveChanges();
                var userArc = new UserArc()
                {
                    UserId = user.Id
                };
                await dbContext.UserArc.AddAsync(userArc);
                dbContext.SaveChanges();


                var userRegisterType = new UserRegisterType()
                {
                    UserId = user.Id,
                    AuthPlatformId = loginUser.AuthPlatformId,
                    RegisterType = loginUser.RegisterType,
                    Email = loginUser.Email,
                    Name = loginUser.Name
                };
                await dbContext.UserRegisterType.AddAsync(userRegisterType);
                dbContext.SaveChanges();

                // Commit transaction if all commands succeed, transaction will auto-rollback
                // when disposed if either commands fails
                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex,null);
                transaction.Rollback();
                return false;
            }
        }

        public User GetUserById(long id)
        {
            return dbContext.User.Where(user => user.Id == id).FirstOrDefault();
        }

        public UserArc GetUserArcById(long userId)
        {
            return dbContext.UserArc.Where(userArc => userArc.UserId == userId).FirstOrDefault();
        }

        public UserRegisterType GetUserRegisterTypeById(long userId)
        {
            return dbContext.UserRegisterType.Where(userRegisterType => userRegisterType.UserId == userId).FirstOrDefault();
        }
        public async Task<UserInfoView> GetUserInfo(UserRegisterType loginUser)
        {

            var list = await dbContext.UserInfoView.Where(user => user.AuthPlatformId == loginUser.AuthPlatformId && user.RegisterType == loginUser.RegisterType).ToListAsync();

            var loggedingUser = list.FirstOrDefault();
            return loggedingUser;
        }

        public UserInfoView GetUserInfoById(long id)
        {
            return dbContext.UserInfoView.Where(userInfo => userInfo.Id == id).FirstOrDefault();
        }

        public bool ReigsterUser(User user,UserArc userArc,UserRegisterType userRegisterType)
        {
            dbContext.User.Update(user);
            dbContext.UserArc.Update(userArc);
            dbContext.UserRegisterType.Update(userRegisterType);

            dbContext.SaveChanges();

            return true;
        }

        public async Task<dynamic> VerifyUserInfo(string accessToken, string permissions, UserRegisterType loginUser)
        {

            switch ((LoginType)loginUser.RegisterType)
            {
                case LoginType.FB:
                    IAuthService authService = authServices.Single(service => service.LoginType == LoginType.FB);
                    return await authService.VerifyUserInfo(accessToken, permissions, loginUser);
                default:
                    return null;
            }
        }

        public UserArc UpdateUserArc(UserArc modifyUserArc,UserArc originalUserArc)
        {
            dbContext.Entry(originalUserArc).CurrentValues.SetValues(modifyUserArc);
            dbContext.Entry(originalUserArc).Property(x => x.UserId).IsModified = false;
            dbContext.Entry(originalUserArc).Property(x => x.CreateTime).IsModified = false;
            originalUserArc.UpdateTime = DateTime.UtcNow;
            dbContext.SaveChanges();
            return modifyUserArc;
        }

        public bool ChangeKycStatusByUserId(KycStatusEnum kycStatus, long userId)
        {
            User user = dbContext.User.Where(user => user.Id == userId).FirstOrDefault();
            UserArc userArc = dbContext.UserArc.Where(userArc => userArc.UserId == userId).FirstOrDefault();

            if (user == null || userArc == null)
            {
                logger.LogError("userId {userId} does not exists", userId);
                return false;
            }

            userArc.KycStatus = (short)kycStatus;
            userArc.KycStatusUpdateTime = DateTime.UtcNow;


            dbContext.SaveChanges();

            return true;
        }

        public void UpdatePicFileNameOfUserInfo(UserArc userArc, PictureTypeEnum pictureType, String fileName)
        {
   
            switch (pictureType)
            {
                case PictureTypeEnum.Front:
                    userArc.IdImageA = fileName;
                    break;
                case PictureTypeEnum.Back:
                    userArc.IdImageB = fileName;
                    break;
            }
            dbContext.UserArc.Update(userArc);
            dbContext.SaveChanges();
        }

        public void UpdatePicFileNameOfDraftRemit(RemitRecord remitRecord, PictureTypeEnum pictureType, String fileName)
        {
            switch (pictureType)
            {
                case PictureTypeEnum.Instant:
                    remitRecord.RealTimePic = fileName;
                    break;
                case PictureTypeEnum.Signature:
                    remitRecord.ESignature = fileName;
                    break;
            }
            remitRecordService.ModifyRemitRecord(remitRecord,null);

        }

        public async Task SystemVerifyArcForRegisterProcess(long userId)
        {
            UserArc userArc = GetUserArcById(userId);
            if (userArc.ArcIssueDate == null || userArc.ArcExpireDate == null)
            {
                logger.LogError("ARC data error");
                throw new Exception("ARC Data not sufficient");
            }
            ArcValidationResult arcValidationResult = arcValidationTask.Validate(workingRootPath, userArc.ArcNo, ((DateTime)userArc.ArcIssueDate).ToString("yyyyMMdd"), ((DateTime)userArc.ArcExpireDate).ToString("yyyyMMdd"), userArc.BackSequence).Result;
            logger.LogInformation("IsSuccessful:{1}", arcValidationResult.IsSuccessful);
            logger.LogInformation("result:{1}", arcValidationResult.Result);

            if (arcValidationResult.IsSuccessful)
            {
                ArcScanRecord record = new ArcScanRecord()
                {
                    ArcStatus = (short)SystemArcVerifyStatusEnum.PASS,
                    ScanTime = DateTime.UtcNow,
                    Description = arcValidationResult.Result,
                    Event = (byte)ArcScanEvent.Register
                };


                userArc.KycStatus = (short)KycStatusEnum.ARC_PASS_VERIFY;
                userArc.KycStatusUpdateTime = DateTime.UtcNow;
                AddScanRecordAndUpdateUserKycStatus(record, userArc);
                // send mail
                await SendMailForRegisterArc(true, receivers, userId);

            }
            else
            {
                ArcScanRecord record = new ArcScanRecord()
                {
                    ArcStatus = (short)SystemArcVerifyStatusEnum.FAIL,
                    ScanTime = DateTime.UtcNow,
                    Description = arcValidationResult.Result,
                    Event = (byte)ArcScanEvent.Register
                };
                userArc.KycStatus = (short)KycStatusEnum.FAILED_KYC;
                userArc.KycStatusUpdateTime = DateTime.UtcNow;
                AddScanRecordAndUpdateUserKycStatus(record, userArc);
                // send mail
                await SendMailForRegisterArc(false, receivers, userId);
            }
        }



        private void AddScanRecordAndUpdateUserKycStatus(ArcScanRecord record, UserArc userArc)
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
                        userArc.KycStatus = (short)KycStatusEnum.AML_PASS_VERIFY;
                    }
                }
                //
                arcScanRecordService.AddScanRecord(record);
                userArc.LastArcScanRecordId = record.Id;
                userArc.UpdateTime = DateTime.UtcNow;
                dbContext.UserArc.Update(userArc);
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

        private async Task SendMailForRegisterArc(bool isSuccessful,string [] mails,long userId)
        {
            Console.WriteLine("Send mail.......");
            SendEmailVo emailVo = new SendEmailVo(); 
            if (isSuccessful)
            {
                emailVo.Subject = REGISTER_ARC_PASSED_SUBJECT;
                emailVo.Message = $"{userId}" + REGISTER_ARC_PASSED_MESSAGE;
            }
            else
            {
                emailVo.Subject = REGISTER_ARC_NOT_PASSED_SUBJECT;
                emailVo.Message = $"使用者id:{userId}" + REGISTER_ARC_NOT_PASSED_MESSAGE;
            }
            foreach (var mail in mails)
            {
                emailVo.Email = mail;
                Response response = await emailSender.SendEmailAsync(emailVo);
            }
        }
        
    }
}
