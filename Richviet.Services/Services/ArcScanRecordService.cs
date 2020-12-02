
using Frontend.DB.EF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RemitRecords.Domains.RemitRecords.Constants;
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

        

        public void AddScanRecord(ArcScanRecord record)
        {
            dbContext.ArcScanRecord.Add(record);
            dbContext.SaveChanges();
        }

        

    }
}
