using System;
using System.Collections.Generic;

namespace Richviet.Services.Models
{
    public partial class ExchangeRate
    {
        public int Id { get; set; }
        public string CurrencyName { get; set; }
        public double Rate { get; set; }
    }
}
