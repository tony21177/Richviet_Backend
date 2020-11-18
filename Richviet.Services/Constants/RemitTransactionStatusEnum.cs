using System;
using System.Collections.Generic;
using System.Text;

namespace Richviet.Services.Constants
{
    public enum RemitTransactionStatusEnum
    {
        OtherError = -10,
        FailedVerified = -9,
        Draft = 0,
        WaitingArcVerifying = 1,
        SuccessfulArcVerification = 2,
        SuccessfulAmlVerification = 3,
        OpConfirmedAndToBePaid = 4,
        Paid = 5,
        Complete = 9
    }
}
