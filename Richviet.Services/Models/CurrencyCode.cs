using System;
using System.Collections.Generic;

namespace Richviet.Services.Models
{
    public partial class CurrencyCode
    {
        public CurrencyCode()
        {
            RemitRecord = new HashSet<RemitRecord>();
        }

        public int Id { get; set; }
        public string CurrencyName { get; set; }
        public string Country { get; set; }
        public double Fee { get; set; }
        public byte FeeType { get; set; }

        public virtual ICollection<RemitRecord> RemitRecord { get; set; }
    }
}
