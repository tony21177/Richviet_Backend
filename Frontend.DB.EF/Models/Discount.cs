using System;
using System.Collections.Generic;

namespace Frontend.DB.EF.Models
{
    public partial class Discount
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public double Value { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public byte UseStatus { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }

        public virtual User User { get; set; }
    }
}
