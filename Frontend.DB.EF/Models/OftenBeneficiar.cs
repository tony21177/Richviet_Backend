using System;
using System.Collections.Generic;

namespace Frontend.DB.EF.Models
{
    public partial class OftenBeneficiar
    {
        public OftenBeneficiar()
        {
            RemitRecord = new HashSet<RemitRecord>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string PayeeAddress { get; set; }
        public string PayeeId { get; set; }
        public string Note { get; set; }
        public long UserId { get; set; }
        public long? ReceiveBankId { get; set; }
        public long PayeeTypeId { get; set; }
        public long PayeeRelationId { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }

        public virtual PayeeRelationType PayeeRelation { get; set; }
        public virtual PayeeType PayeeType { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<RemitRecord> RemitRecord { get; set; }
    }
}
