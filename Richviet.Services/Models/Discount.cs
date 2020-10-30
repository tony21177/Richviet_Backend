using System;
using System.Collections.Generic;

namespace Richviet.Services.Models
{
    public partial class Discount
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public double Value { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public byte UseStatus { get; set; }
        public DateTimeOffset CreateTime { get; set; }
        public DateTimeOffset UpdateTime { get; set; }

        public virtual User User { get; set; }
    }
}
