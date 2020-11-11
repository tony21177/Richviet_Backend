using Microsoft.Extensions.Logging;
using Richviet.Services.Contracts;
using Richviet.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Richviet.Services.Services
{
    public class RemitSettingService : IRemitSettingService
    {

        private readonly ILogger logger;
        private readonly GeneralContext dbContext;

        public RemitSettingService(ILogger<RemitSettingService> logger, GeneralContext dbContext)
        {
            this.logger = logger;
            this.dbContext = dbContext;
        }

        public BussinessUnitRemitSetting GetRemitSettingByCountry(string country)
        {
            return dbContext.BussinessUnitRemitSetting.Where<BussinessUnitRemitSetting>(setting => setting.Country.Equals(country)).FirstOrDefault();
        }
    }
}
