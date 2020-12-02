using System;
using System.Collections.Generic;
using System.Text;
using RemitRecords.Domains.RemitRecords.Command.Adapter.Repositories;

namespace RemitRecords.Domains.RemitRecords.Command.UseCase
{
    public class RemitTransactionStatusModifier
    {
        private readonly IRemitRecordCommandRepository _recordCommandRepository;

        public RemitTransactionStatusModifier(IRemitRecordCommandRepository recordCommandRepository)
        {
            this._recordCommandRepository = recordCommandRepository;
        }

        public void RemitSuccess(long recordId, string comment)
        {
            this._recordCommandRepository.UpdateTransactionStatus(recordId, 9);
        }
    }
}
