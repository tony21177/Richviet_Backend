using System;
using System.Collections.Generic;

namespace Frontend.DB.EF.Models
{
    public partial class UserInfoView
    {
        public long Id { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public string Country { get; set; }
        public string ArcName { get; set; }
        public string ArcNo { get; set; }
        public string PassportId { get; set; }
        public string BackSequence { get; set; }
        public string IdImageA { get; set; }
        public string IdImageB { get; set; }
        public string IdImageC { get; set; }
        public byte? KycStatus { get; set; }
        public DateTime? KycStatusUpdateTime { get; set; }
        public DateTime? RegisterTime { get; set; }
        public string AuthPlatformId { get; set; }
        public int RegisterType { get; set; }
        public string LoginPlatformEmal { get; set; }
        public string Name { get; set; }
    }
}
