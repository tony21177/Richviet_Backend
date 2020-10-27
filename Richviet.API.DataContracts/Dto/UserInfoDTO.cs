using System;
using System.Collections.Generic;

namespace Richviet.API.DataContracts.Dto

{
    public partial class UserInfoDTO
    {
        public int Id { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public byte Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public byte Status { get; set; }
        public string Country { get; set; }
        public string ArcName { get; set; }
        public string ArcNo { get; set; }
        public string PassportId { get; set; }
        public string BackSequence { get; set; }
        public string IdImageA { get; set; }
        public string IdImageB { get; set; }
        public string IdImageC { get; set; }
        public byte? KycStatus { get; set; }
        public DateTimeOffset? KycStatusUpdateTime { get; set; }
        public DateTimeOffset? RegisterTime { get; set; }
        public string AuthPlatformId { get; set; }
        public byte RegisterType { get; set; }
        public string LoginPlatformEmal { get; set; }
        public string Name { get; set; }
    }
}
