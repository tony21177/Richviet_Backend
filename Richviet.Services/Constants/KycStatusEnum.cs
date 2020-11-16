using System;
using System.Collections.Generic;
using System.Text;

namespace Richviet.Services.Constants
{
    public enum KycStatusEnum
    {
        FORBIDDEN = 10,
        FAILED_KYC = 9,
        FAILED_AML = 8,
        DRAFT_MEMBER = 0,
        WAITING_VERIFIED_KYC = 1,
        ARC_PASS_VERIFY = 2,
        AML_PASS_VERIFY = 3,
        PASSED_KYC_FORMAL_MEMBER = 4

    }
}
