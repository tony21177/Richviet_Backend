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

        public List<CurrencyCode> GetCureencyByCountry(string country)
        {
            return dbContext.CurrencyCode.Where(currency => currency.Country == country).ToList<CurrencyCode>();

        }

        public CurrencyCode GetCurrencyById(int id)
        {
            return dbContext.CurrencyCode.Find(id);
        }
    }
}
