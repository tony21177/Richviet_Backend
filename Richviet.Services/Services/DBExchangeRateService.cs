using Microsoft.Extensions.Logging;
using Richviet.Services.Contracts;
using Frontend.DB.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Richviet.Services.Services
{
    public class DBExchangeRateService : IExchangeRateService
    {
        private readonly ILogger logger;
        private readonly GeneralContext dbContext;

        public DBExchangeRateService(ILogger<DBExchangeRateService> logger, GeneralContext dbContext)
        {
            this.logger = logger;
            this.dbContext = dbContext;
        }

        public List<ExchangeRate> GetExchangeRate()
        {
            return dbContext.ExchangeRate.ToList<ExchangeRate>();
        }

        public ExchangeRate GetExchangeRateByCurrencyName(String currencyName)
        {
            return dbContext.ExchangeRate.Where(rate => rate.CurrencyName.Equals(currencyName.ToUpper())).FirstOrDefault();
        }
    }
}
