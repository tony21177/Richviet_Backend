﻿using Frontend.DB.EF.Models;
using RemitRecords.Domains.RemitRecords.Vo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using RemitRecords.Domains.RemitRecords.Constants;

namespace RemitRecords.Domains.RemitRecords.Query
{
    public class RemitRecordQueryRepositories : IRemitRecordQueryRepositories
    {
        private readonly GeneralContext _context;

        public RemitRecordQueryRepositories(GeneralContext context)
        {
            _context = context;
        }

        public RemitAvailableAmountSumVo QueryMonthlyAvailableAmount(long userId,string country)
        {
            BussinessUnitRemitSetting setting = _context.BussinessUnitRemitSetting.Where(setting=>setting.Country.Equals(country.ToUpper())).FirstOrDefault();
            var nowMonth = DateTime.UtcNow.Month;

            var monthlyAvailableAmountList = from u in _context.UserArc.Where(userArc=>userArc.UserId == userId)
                                         join r in _context.RemitRecord.Where(record => record.UserId == userId) on u.UserId equals r.UserId
                                         where (r.TransactionStatus > 0 && ((DateTime)r.FormalApplyTime).Month == nowMonth)
                                         || (r.TransactionStatus >= (short)RemitTransactionStatusEnum.Paid &&  ((DateTime)r.PaymentTime).Month == nowMonth)
                                         || (r.TransactionStatus > 0 && r.TransactionStatus < (short)RemitTransactionStatusEnum.Paid && ((DateTime)r.FormalApplyTime).Month == nowMonth-1)
                                         group r by (r.UserId) into g
                                         select new {
                                             UserId = g.Key,
                                             MonthlyAvailableRemitAmount = (int)(setting.MonthlyMax - g.Sum(ele=>ele.FromAmount))
                                         };
            var monthlyAvailableAmount = monthlyAvailableAmountList.FirstOrDefault();
            if (monthlyAvailableAmount == null)
            {
                return new RemitAvailableAmountSumVo
                {
                    UserId = userId,
                    DailyAvailableRemitAmount = setting.DailyMax,
                    MonthlyAvailableRemitAmount = setting.MonthlyMax,
                    YearlyAvailableRemitAmount = setting.YearlyMax
                };
            }
            RemitAvailableAmountSumVo remitAvailableAmountSumVo = new RemitAvailableAmountSumVo
            {
                UserId = monthlyAvailableAmount.UserId,
                MonthlyAvailableRemitAmount = monthlyAvailableAmount.MonthlyAvailableRemitAmount
            };

            return remitAvailableAmountSumVo;
        }



        public RemitAvailableAmountSumVo QueryYearlyAvailableAmount(long userId, string country)
        {
            throw new NotImplementedException();
        }
    }
}
