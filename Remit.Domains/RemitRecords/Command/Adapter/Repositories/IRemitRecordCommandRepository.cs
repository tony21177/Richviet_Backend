using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.SqlServer.Migrations.Internal;

namespace RemitRecords.Domains.RemitRecords.Command.Adapter.Repositories
{
    public interface IRemitRecordCommandRepository
    {
        void UpdateTransactionStatus(long recordId, short transactionStatus, string comment);
    }
}
