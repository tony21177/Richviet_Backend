
using Frontend.DB.EF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Richviet.Services.Constants;
using Richviet.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Richviet.Services.Services
{
    public class ArcScanRecordService : IArcScanRecordService
    {
        private readonly ILogger logger;
        private readonly GeneralContext dbContext;
        private readonly IConfiguration configuration;

        public ArcScanRecordService(ILogger<ArcScanRecordService> logger, GeneralContext dbContext, IConfiguration configuration)
        {
            this.logger = logger;
            this.dbContext = dbContext;
            this.configuration = configuration;
        }

        public void AddScanRecordForRegiterProcess(ArcScanRecord record, UserArc userArc)
        {

            using var transaction = dbContext.Database.BeginTransaction();
            try
            {
                // for demo
                if(configuration["IsDemo"]!=null && bool.Parse(configuration["IsDemo"]) == true)
                {
                    if (userArc.ArcNo.Equals("ZZ00000000"))
                    {
                        
                    }else if (userArc.ArcNo.Equals("ZZ11111111"))
                    {
                        userArc.KycStatus = (short)KycStatusEnum.AML_PASS_VERIFY;
                    }
                }
                //
                dbContext.ArcScanRecord.Add(record);
                dbContext.SaveChanges();
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

        public void AddScanRecordForRemitProcess(ArcScanRecord record, UserArc userArc,RemitRecord remitRecord)
        {
            using var transaction = dbContext.Database.BeginTransaction();
            try
            {
                // for demo
                if (configuration["IsDemo"] != null && bool.Parse(configuration["IsDemo"]) == true)
                {
                    if (userArc.ArcNo.Equals("ZZ00000000"))
                    {

                    }
                    else if (userArc.ArcNo.Equals("ZZ11111111"))
                    {
                        userArc.KycStatus = (short)KycStatusEnum.AML_PASS_VERIFY;
                        remitRecord.TransactionStatus = (short)RemitTransactionStatusEnum.SuccessfulAmlVerification;
                    }
                }
                //
                dbContext.ArcScanRecord.Add(record);
                dbContext.SaveChanges();
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

    }
}
