using System;
using System.Collections.Generic;

namespace Frontend.DB.EF.Models
{
    public partial class RemitAdminReviewLog
    {
        public long Id { get; set; }
        public long RemitRecordId { get; set; }
        public short FromTransactionStatus { get; set; }
        public short ToTransactionStatus { get; set; }
        public string Note { get; set; }
        public DateTime CreateTime { get; set; }

        public virtual RemitRecord RemitRecord { get; set; }
    }
}
