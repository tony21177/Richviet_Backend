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

        public List<BussinessUnitRemitSetting> GetRemitSettingList()
        {
            return dbContext.BussinessUnitRemitSetting.ToList();
        }

        public bool AddRemitSetting(BussinessUnitRemitSetting remitSetting)
        {
            try
            {
                dbContext.BussinessUnitRemitSetting.Add(remitSetting);
                dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                logger.LogDebug(ex.Message);
            }
            return false;
        }

        public bool DeleteRemitSetting(int id)
        {
            try
            {
                BussinessUnitRemitSetting remitSetting = dbContext.BussinessUnitRemitSetting.Single(x => x.Id == id);
                if (remitSetting != null)
                {
                    dbContext.BussinessUnitRemitSetting.Remove(remitSetting);
                    dbContext.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                logger.LogDebug(ex.Message);
            }
            return false;
        }

        public bool ModifyRemitSetting(BussinessUnitRemitSetting modifyRemitSetting)
        {
            try
            {
                BussinessUnitRemitSetting remitSetting = dbContext.BussinessUnitRemitSetting.Single(x => x.Id == modifyRemitSetting.Id);
                dbContext.Entry(remitSetting).CurrentValues.SetValues(modifyRemitSetting);
                dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                logger.LogDebug(ex.Message);
            }
            return false;
        }
    }
}
