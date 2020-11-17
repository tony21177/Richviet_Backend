using System;
using System.Collections.Generic;
using System.Text;

namespace Richviet.Services.Constants
{
    public enum RemitTransactionStatusEnum
    {
        OtherError = -10,
        Draft = 0,
        FailedVerified = -9,
        WaitingVerifying = 1,
        WaitingPaying = 2,
        Paid = 3,
        Complete = 4
    }
}
