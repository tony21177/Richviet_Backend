﻿using System.Linq;
using Frontend.DB.EF.Models;

namespace RemitRecords.Domains.RemitRecords.Command.Adapter.Repositories
{
    public class  RemitRecordCommandDbCommandRepository : IRemitRecordCommandRepository
    {
        private readonly GeneralContext _context;

        public RemitRecordCommandDbCommandRepository(GeneralContext context)
        {
            _context = context;
        }

        public void UpdateTransactionStatus(long recordId, short transactionStatus,string comment)
        {
            var record =  _context.RemitRecord.SingleOrDefault(x => x.Id == recordId);
            _context.Entry(record).Reference(record => record.ToCurrency).Load();
            var exchangeRate = _context.ExchangeRate.SingleOrDefault(x => x.CurrencyName.Equals(record.ToCurrency.CurrencyName));
            var originalTransactionStatus = record.TransactionStatus;
            if (record == null) return;
            record.TransactionStatus = transactionStatus;
            record.TransactionExchangeRate = exchangeRate.Rate;
            RemitAdminReviewLog reviewLog = new RemitAdminReviewLog()
            {
                RemitRecordId = recordId,
                FromTransactionStatus = originalTransactionStatus,
                ToTransactionStatus = transactionStatus,
                Note = comment
            };
            record.RemitAdminReviewLog.Add(reviewLog);
            _context.Update(record);
            _context.SaveChanges();
        }
    }
}