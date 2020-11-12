using Microsoft.Extensions.Logging;
using Richviet.Services.Contracts;
using Richviet.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Richviet.Services.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ILogger logger;
        private readonly GeneralContext dbContext;

        public CurrencyService(ILogger<CurrencyService> logger, GeneralContext dbContext)
        {
            this.logger = logger;
            this.dbContext = dbContext;
        }

        public List<CurrencyCode> GetCurrencyByCountry(string country)
        {
            return dbContext.CurrencyCode.Where(currency => currency.Country == country).ToList<CurrencyCode>();
        }

        public List<CurrencyCode> GetCurrencyList()
        {
            return dbContext.CurrencyCode.ToList();
        }

        public bool AddCurrency(CurrencyCode currency)
        {
            try
            {
                dbContext.CurrencyCode.Add(currency);
                dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                logger.LogDebug(ex.Message);
            }
            return false;
        }

        public bool DeleteCurrency(int id)
        {
            try
            {
                CurrencyCode currency = dbContext.CurrencyCode.Single(x => x.Id == id);
                if (currency != null)
                {
                    dbContext.CurrencyCode.Remove(currency);
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
        
        public bool ModifyCurrency(CurrencyCode modifyCurrency)
        {
            try
            {
                CurrencyCode currency = dbContext.CurrencyCode.Single(x => x.Id == modifyCurrency.Id);
                dbContext.Entry(currency).CurrentValues.SetValues(modifyCurrency);
                dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                logger.LogDebug(ex.Message);
            }
            return false;
        }

        public CurrencyCode GetCurrencyById(int id)
        {
            return dbContext.CurrencyCode.Find(id);
        }
    }
}
