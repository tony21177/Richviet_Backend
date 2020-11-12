using System;
using System.Collections.Generic;

namespace Frontend.DB.EF.Models
{
    public partial class RemitRecord
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string ArcName { get; set; }
        public string ArcNo { get; set; }
        public int PayeeType { get; set; }
        public string IdImageA { get; set; }
        public string IdImageB { get; set; }
        public string IdImageC { get; set; }
        public string RealTimePic { get; set; }
        public string ESignature { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public int? FromCurrencyId { get; set; }
        public long? ToCurrencyId { get; set; }
        public double FromAmount { get; set; }
        public double ApplyExchangeRate { get; set; }
        public double TransactionExchangeRate { get; set; }
        public double Fee { get; set; }
        public int FeeType { get; set; }
        public int? DiscountId { get; set; }
        public double? DiscountAmount { get; set; }
        public long? BeneficiarId { get; set; }
        public int TransactionStatus { get; set; }
        public int? ArcStatus { get; set; }
        public DateTime? ArcVerifyTime { get; set; }
        public DateTime? PaymentTime { get; set; }
        public string PaymentCode { get; set; }

        public virtual OftenBeneficiar Beneficiar { get; set; }
        public virtual CurrencyCode ToCurrency { get; set; }
        public virtual User User { get; set; }
    }
}
