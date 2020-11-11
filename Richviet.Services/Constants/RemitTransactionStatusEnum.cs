using Org.BouncyCastle.Asn1.Esf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Richviet.Services.Constants
{
    public enum RemitTransactionStatusEnum
    {
        OtherError = 99,
        Draft = 98,
        FailedVerified = 9,
        WaitingVerifying = 0,
        WaitingPaying = 1,
        Paid = 2,
        Complete = 3
    }
}
