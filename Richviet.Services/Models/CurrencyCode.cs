using System;
using System.Collections.Generic;

namespace Richviet.Services.Models
{
    public partial class CurrencyCode
    {
        public int Id { get; set; }
        public string CurrencyName { get; set; }
        public string Country { get; set; }
        public double Rate { get; set; }
        public double CommisionRate { get; set; }
    }
}
