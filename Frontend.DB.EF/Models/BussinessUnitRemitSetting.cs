using System;
using System.Collections.Generic;

namespace Frontend.DB.EF.Models
{
    public partial class BussinessUnitRemitSetting
    {
        public long Id { get; set; }
        public string Country { get; set; }
        public double RemitMin { get; set; }
        public double RemitMax { get; set; }
        public DateTime UpdateTime { get; set; }
        public double? DailyMax { get; set; }
        public double? MonthlyMax { get; set; }
        public double? YearlyMax { get; set; }
    }
}
