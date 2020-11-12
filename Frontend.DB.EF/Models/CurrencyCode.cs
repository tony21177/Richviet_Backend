using System;
using System.Collections.Generic;

namespace Frontend.DB.EF.Models
{
    public partial class CurrencyCode
    {
        public CurrencyCode()
        {
            RemitRecord = new HashSet<RemitRecord>();
        }

        public long Id { get; set; }
        public string CurrencyName { get; set; }
        public string Country { get; set; }
        public double Fee { get; set; }
        public int FeeType { get; set; }

        public virtual ICollection<RemitRecord> RemitRecord { get; set; }
    }
}
