using System;
using System.Collections.Generic;

namespace Richviet.Services.Models
{
    public partial class ArcScanRecord
    {
        public ArcScanRecord()
        {
            RemitRecord = new HashSet<RemitRecord>();
            UserArc = new HashSet<UserArc>();
        }

        public int Id { get; set; }
        public byte ArcStatus { get; set; }
        public DateTime ScanTime { get; set; }

        public virtual ICollection<RemitRecord> RemitRecord { get; set; }
        public virtual ICollection<UserArc> UserArc { get; set; }
    }
}
