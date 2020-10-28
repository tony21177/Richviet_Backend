using System;
using System.Collections.Generic;

namespace Richviet.Services.Models
{
    public partial class RemitRecord
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ArcName { get; set; }
        public string ArcNo { get; set; }
        public byte PayeeType { get; set; }
        public string IdImageA { get; set; }
        public string IdImageB { get; set; }
        public string IdImageC { get; set; }
        public string RealTimePic { get; set; }
        public string ESignature { get; set; }
        public DateTimeOffset? CreateTime { get; set; }
        public DateTimeOffset? UpdateTime { get; set; }
        public int FromCurrencyId { get; set; }
        public int ToCurrencyId { get; set; }
        public double FromAmount { get; set; }
        public double ApplyExchangeRate { get; set; }
        public double TransactionExchangeRate { get; set; }
        public double Fee { get; set; }
        public byte FeeType { get; set; }
        public int? DiscountId { get; set; }
        public double? DiscountAmount { get; set; }
        public byte TransactionStatus { get; set; }
        public int? BeneficiarId { get; set; }

        public virtual OftenBeneficiar Beneficiar { get; set; }
        public virtual User User { get; set; }
    }
}
