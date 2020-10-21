using System;
using System.Collections.Generic;

namespace Richviet.Services.Models
{
    public partial class UserArc
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Country { get; set; }
        public string ArcName { get; set; }
        public string ArcNo { get; set; }
        public string IdImageA { get; set; }
        public string IdImageB { get; set; }
        public string IdImageC { get; set; }
        public int? KycStatus { get; set; }
        public DateTimeOffset? KycStatusUpdateTime { get; set; }
        public DateTimeOffset? CreateTime { get; set; }
        public DateTimeOffset? UpdateTime { get; set; }
    }
}
