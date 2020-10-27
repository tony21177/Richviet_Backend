using System;
using System.Collections.Generic;
using System.Text;

namespace Richviet.API.DataContracts.Dto
{
    public class ExchangeRateDTO
    {
        public CurrencyRate[] exchangeRates { get; set; }

        public class CurrencyRate
        {
            public String currencyName { get; set; }

            public double rate { get; set; }
        }
    }

    
}
