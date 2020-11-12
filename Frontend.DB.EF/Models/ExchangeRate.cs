using System;
using System.Collections.Generic;

namespace Frontend.DB.EF.Models
{
    public partial class ExchangeRate
    {
        public long Id { get; set; }
        public string CurrencyName { get; set; }
        public double Rate { get; set; }
    }
}
