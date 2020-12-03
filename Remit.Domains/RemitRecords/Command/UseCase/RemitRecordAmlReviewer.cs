using System;
using System.Collections.Generic;
using System.Text;
using RemitRecords.Domains.RemitRecords.Command.Adapter.Repositories;

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
            //3 is AML pass
            recordCommandRepository.UpdateTransactionStatus(remitRecordId, 3);
        }

        public void AmlReviewFail(long remitRecordId, string comment)
        {
            //-8 Aml fail
            recordCommandRepository.UpdateTransactionStatus(remitRecordId, -8);
        }
    }
}
