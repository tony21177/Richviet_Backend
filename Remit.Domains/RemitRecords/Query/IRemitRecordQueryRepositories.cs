using RemitRecords.Domains.RemitRecords.Vo;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemitRecords.Domains.RemitRecords.Query
{
    public interface IRemitRecordQueryRepositories
    {
        RemitAvailableAmountSumVo QueryMonthlyAvailableAmount(long userId, string country);

        RemitAvailableAmountSumVo QueryYearlyAvailableAmount(long userId, string country);

    }
}
