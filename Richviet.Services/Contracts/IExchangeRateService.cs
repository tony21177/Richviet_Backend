using Frontend.DB.EF.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Richviet.Services.Contracts
{
    public interface IExchangeRateService
    {
        List<ExchangeRate> GetExchangeRate();

        ExchangeRate GetExchangeRateByCurrencyName(String currencyName);

    }
}
