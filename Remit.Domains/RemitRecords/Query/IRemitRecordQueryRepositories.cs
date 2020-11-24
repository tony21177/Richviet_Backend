using RemitRecords.Domains.RemitRecords.Vo;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemitRecords.Domains.RemitRecords.Query
{
    public interface IRemitRecordQueryRepositories
    {
         RemitRecordsDailySumVo QueryDailyRemitValidAmount(long userId);

         RemitRecordsMonthlySumVo QueryMonthlyRemitValidAmount(long userId);

    }
}
