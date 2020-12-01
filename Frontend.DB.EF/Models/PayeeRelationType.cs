using System;
using System.Collections.Generic;

namespace Frontend.DB.EF.Models
{
    public partial class PayeeRelationType
    {
        public PayeeRelationType()
        {
            OftenBeneficiary = new HashSet<OftenBeneficiary>();
        }

        public long Id { get; set; }
        public byte Type { get; set; }
        public string Description { get; set; }

        public virtual ICollection<OftenBeneficiary> OftenBeneficiary { get; set; }
    }
}
