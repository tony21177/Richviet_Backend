using System;
using System.Collections.Generic;

namespace Richviet.Services.Models
{
    public partial class UserInfoView
    {
        public int Id { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTimeOffset? CreateTime { get; set; }
        public DateTimeOffset? UpdateTime { get; set; }
        public string Country { get; set; }
        public string ArcName { get; set; }
        public string ArcNo { get; set; }
        public string IdImageA { get; set; }
        public string IdImageB { get; set; }
        public string IdImageC { get; set; }
        public byte? KycStatus { get; set; }
        public DateTimeOffset? KycStatusUpdateTime { get; set; }
        public string AuthPlatformId { get; set; }
        public byte RegisterType { get; set; }
        public string FbEmal { get; set; }
        public string Name { get; set; }
    }
}
