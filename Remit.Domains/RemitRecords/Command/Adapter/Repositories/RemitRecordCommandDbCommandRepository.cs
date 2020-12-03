using System.Linq;
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

        public void UpdateTransactionStatus(long recordId, short transactionStatus)
        {
            var record =  _context.RemitRecord.SingleOrDefault(x => x.Id == recordId);
            if (record == null) return;
            record.TransactionStatus = transactionStatus;
            _context.Update(record);

        }
    }
}