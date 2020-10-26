using System;
using System.Collections.Generic;

namespace Richviet.Services.Models
{
    public partial class OftenBeneficiar
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PayeeAddress { get; set; }
        public string PayeeId { get; set; }
        public string Note { get; set; }
        public int UserId { get; set; }
        public int? ReceiveBankId { get; set; }
        public int PayeeTypeId { get; set; }
        public int PayeeRelationId { get; set; }
        public DateTimeOffset? CreateTime { get; set; }
        public DateTimeOffset? UpdateTime { get; set; }

        public virtual PayeeRelationType PayeeRelation { get; set; }
        public virtual PayeeType PayeeType { get; set; }
        public virtual User User { get; set; }
    }
}
