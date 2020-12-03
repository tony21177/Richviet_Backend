using System;
using System.Collections.Generic;
using System.Text;
using RemitRecords.Domains.RemitRecords.Command.Adapter.Repositories;
using RemitRecords.Domains.RemitRecords.Constants;

namespace RemitRecords.Domains.RemitRecords.Command.UseCase
{
    public class RemitRecordAmlReviewer
    {
        private readonly IRemitRecordCommandRepository recordCommandRepository;

        public RemitRecordAmlReviewer(IRemitRecordCommandRepository recordCommandRepository)
        {
            this.recordCommandRepository = recordCommandRepository;
        }

        public void AmlReviewPass(long remitRecordId, string comment)
        {
            recordCommandRepository.UpdateTransactionStatus(remitRecordId, (short)RemitTransactionStatusEnum.OpConfirmedAndToBePaid, comment);
        }

        public void AmlReviewFail(long remitRecordId, string comment)
        {
            
            recordCommandRepository.UpdateTransactionStatus(remitRecordId, (short)RemitTransactionStatusEnum.FailedVerified, comment);
        }
    }
}
