using System;
using System.Collections.Generic;
using System.Text;
using RemitRecords.Domains.RemitRecords.Command.Adapter.Repositories;
using RemitRecords.Domains.RemitRecords.Constants;

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
            this._recordCommandRepository.UpdateTransactionStatus(recordId, (short)RemitTransactionStatusEnum.Complete,comment);
        }

        public void RemitFail(long recordId, string comment)
        {
            this._recordCommandRepository.UpdateTransactionStatus(recordId, (short)RemitTransactionStatusEnum.OtherError,comment);
        }
    }
}
