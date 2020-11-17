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
        WaitingArcVerifying = 1,
        WaitingAmlVerifying = 2,
        WaitingPaying = 3,
        Paid = 4,
        Complete = 5
    }
}
