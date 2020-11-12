using System;
using System.Collections.Generic;

namespace Frontend.DB.EF.Models
{
    public partial class PayeeRelationType
    {
        public PayeeRelationType()
        {
            OftenBeneficiar = new HashSet<OftenBeneficiar>();
        }

        public long Id { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }

        public virtual ICollection<OftenBeneficiar> OftenBeneficiar { get; set; }
    }
}
