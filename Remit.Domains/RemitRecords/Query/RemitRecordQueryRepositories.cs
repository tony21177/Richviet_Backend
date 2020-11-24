using Frontend.DB.EF.Models;
using RemitRecords.Domains.RemitRecords.Vo;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemitRecords.Domains.RemitRecords.Query
{
    public class RemitRecordQueryRepositories : IRemitRecordQueryRepositories
    {
        private readonly GeneralContext _context;

        public RemitRecordQueryRepositories(GeneralContext context)
        {
            _context = context;
        }

        public RemitRecordsDailySumVo QueryDailyRemitValidAmount(long userId)
        {
            //var todayValidRemit = from u in _context.UserArc
            //                      join r in _context.RemitRecord on u.UserId equals r.UserId
            //                      where r.TransactionStatus = RemitTransa
            throw new NotImplementedException();
        }

        public RemitRecordsMonthlySumVo QueryMonthlyRemitValidAmount(long userId)
        {
            throw new NotImplementedException();
        }
    }
}
