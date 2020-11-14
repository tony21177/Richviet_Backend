using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Richviet.Services.Constants;
using Richviet.Services.Contracts;
using Frontend.DB.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Richviet.Services.Services
{
    public class RemitRecordService : IRemitRecordService
    {

        private readonly ILogger logger;
        private readonly GeneralContext dbContext;
        private readonly IUserService userService;

        public RemitRecordService(ILogger<RemitRecordService> logger, GeneralContext dbContext, IUserService userService)
        {
            this.logger = logger;
            this.dbContext = dbContext;
            this.userService = userService;
        }


        public RemitRecord CreateRemitRecordByUserId(int userId,PayeeTypeEnum payeeTypeEnum)
        {
            UserArc userArc = userService.GetUserArcById(userId);

            RemitRecord newRemitRecord = new RemitRecord()
            {
                UserId = userId,
                ArcName = userArc.ArcName,
                ArcNo = userArc.ArcNo,
                PayeeType = (byte)payeeTypeEnum,
            };
            dbContext.RemitRecord.Add(newRemitRecord);
            dbContext.SaveChanges();
            return newRemitRecord;
        }

        public RemitRecord GetOngoingRemitRecordByUserId(int userId)
        {
            byte[] completedStatus = 
            {
                (byte)RemitTransactionStatusEnum.Complete,(byte)RemitTransactionStatusEnum.FailedVerified,(byte)RemitTransactionStatusEnum.OtherError
            };
            List<byte> completedStatusList = completedStatus.ToList();
            return dbContext.RemitRecord.Where<RemitRecord>(record => record.UserId==userId&& !completedStatusList.Contains(record.TransactionStatus)).FirstOrDefault();
        }

        public RemitRecord GetRemitRecordById(int id)
        {
            RemitRecord record = dbContext.RemitRecord.Find(id);
            dbContext.Entry(record).Reference(record=>record.Beneficiar).Query()
            .Include(beneficiar => beneficiar.PayeeRelation)
            .Load();
            dbContext.Entry(record).Reference(record => record.ToCurrency).Load();
            return record;
        }

        public RemitRecord ModifyRemitRecord(RemitRecord modifiedRemitRecord)
        {
            dbContext.RemitRecord.Update(modifiedRemitRecord);
            dbContext.SaveChanges();
            dbContext.Entry(modifiedRemitRecord).Reference(record => record.Beneficiar).Query()
            .Include(beneficiar => beneficiar.PayeeRelation)
            .Load();
            return modifiedRemitRecord;
        }
    }
}
