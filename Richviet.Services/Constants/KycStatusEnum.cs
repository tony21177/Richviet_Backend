using Renci.SshNet.Messages.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Richviet.Services.Constants
{
    public enum KycStatusEnum
    {
        FAILED_KYC = 9,
        NOT_VERIFIED_KYC = 0,
        WAITING_VERIFIED_KYC = 1,
        PASSED_KYC = 2
    }
}
