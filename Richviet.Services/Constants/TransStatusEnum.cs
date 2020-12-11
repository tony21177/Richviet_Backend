using System;
using System.Collections.Generic;
using System.Text;

namespace Richviet.Services.Constants
{
    public enum TransStatusEnum
    {
        KYC_REVIEW = 1,
        AML_REVIEW = 2,
        STAFF_REVIEW = 3,
        PENDING_PAY = 4,
        PENDING_REMIT = 5,
        FINISH = 9,
        Overdue = -7,
        AML_REVIEW_FAIL = -8,
        REVIEW_FAIL = -9,
        OTHER_ERROR = -10
    }
}
