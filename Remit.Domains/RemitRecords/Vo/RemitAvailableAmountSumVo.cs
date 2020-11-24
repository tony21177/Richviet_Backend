using System;
using System.Collections.Generic;
using System.Text;

namespace RemitRecords.Domains.RemitRecords.Vo
{
    public class RemitAvailableAmountSumVo
    {
        public long UserId { get; set; }
        public double? DailyAvailableRemitAmount { get; set; }
        public double? MonthlyAvailableRemitAmount { get; set; }
        public double? YearlyAvailableRemitAmount { get; set; }

    }
}
