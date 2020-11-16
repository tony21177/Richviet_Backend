using Frontend.DB.EF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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

        public ArcScanRecordService(ILogger<ArcScanRecordService> logger, GeneralContext dbContext)
        {
            this.logger = logger;
            this.dbContext = dbContext;
        }

        public void AddScanRecordForRegiterProcess(ArcScanRecord record, UserArc userArc)
        {
            //dbContext.Entry(record).State = EntityState.Added;
            //dbContext.Entry(userArc).State = EntityState.Modified;
            //userArc.LastArcScanRecord = record;
            //userArc.UpdateTime = DateTime.UtcNow;
            //dbContext.UserArc.Update(userArc);
            //dbContext.SaveChanges();

            using var transaction = dbContext.Database.BeginTransaction();
            try
            {

                dbContext.ArcScanRecord.Add(record);
                dbContext.SaveChanges();
                userArc.LastArcScanRecordId = record.Id;       
                userArc.UpdateTime = DateTime.UtcNow;           
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
