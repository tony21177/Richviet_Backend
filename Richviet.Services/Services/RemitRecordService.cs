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

namespace Richviet.Services.Services
{
    public class RemitRecordService : IRemitRecordService
    {

        private readonly ILogger logger;
        private readonly GeneralContext dbContext;

        public RemitRecordService(ILogger<RemitRecordService> logger, GeneralContext dbContext)
        {
            this.logger = logger;
            this.dbContext = dbContext;
        }


        public RemitRecord CreateRemitRecordByUserArc(UserArc userArc,RemitRecord remitRecord, PayeeTypeEnum payeeTypeEnum)
        {
            remitRecord.UserId = userArc.UserId;
            remitRecord.ArcName = userArc.ArcName;
            remitRecord.ArcNo = userArc.ArcNo;
            remitRecord.PayeeType = (byte)payeeTypeEnum;
            
            dbContext.RemitRecord.Add(remitRecord);
            dbContext.SaveChanges();
            dbContext.Entry(remitRecord).Reference(record => record.Beneficiar).Query()
                .Include(beneficiar => beneficiar.PayeeRelation)
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
                dbContext.Entry(record).Reference(record => record.Beneficiar).Query()
                .Include(beneficiar => beneficiar.PayeeRelation)
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
            dbContext.Entry(modifiedRemitRecord).Reference(record => record.Beneficiar).Query()
            .Include(beneficiar => beneficiar.PayeeRelation)
            .Load();
            dbContext.Entry(modifiedRemitRecord).Reference(record => record.ToCurrency).Load();
            return modifiedRemitRecord;
        }

        public List<RemitRecord> GetRemitRecordsByUserId(long userId)
        {
            return dbContext.RemitRecord.Include(record=>record.Beneficiar).ThenInclude(beneficiar=>beneficiar.PayeeRelation).Include("ToCurrency").Where(record => record.UserId == userId).ToList();
        }



        public void DeleteRmitRecord(RemitRecord record)
        {
            dbContext.RemitRecord.Remove(record);
            dbContext.SaveChanges();
        }
    }
}
