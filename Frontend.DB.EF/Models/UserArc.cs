using System;
using System.Collections.Generic;

namespace Frontend.DB.EF.Models
{
    public partial class UserArc
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Country { get; set; }
        public string ArcName { get; set; }
        public string ArcNo { get; set; }
        public string PassportId { get; set; }
        public string BackSequence { get; set; }
        public DateTime? ArcIssueDate { get; set; }
        public DateTime? ArcExpireDate { get; set; }
        public string IdImageA { get; set; }
        public string IdImageB { get; set; }
        public string IdImageC { get; set; }
        public short KycStatus { get; set; }
        public DateTime? KycStatusUpdateTime { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public long? LastArcScanRecordId { get; set; }

        public virtual ArcScanRecord LastArcScanRecord { get; set; }
        public virtual User User { get; set; }
    }
}
