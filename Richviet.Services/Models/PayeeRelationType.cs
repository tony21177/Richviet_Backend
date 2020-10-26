using System;
using System.Collections.Generic;

namespace Richviet.Services.Models
{
    public partial class PayeeRelationType
    {
        public PayeeRelationType()
        {
            OftenBeneficiar = new HashSet<OftenBeneficiar>();
        }

        public int Id { get; set; }
        public byte Type { get; set; }
        public string Description { get; set; }

        public virtual ICollection<OftenBeneficiar> OftenBeneficiar { get; set; }
    }
}
