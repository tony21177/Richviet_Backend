using System;
using System.Collections.Generic;

namespace Frontend.DB.EF.Models
{
    public partial class RemitRecord
    {
        public RemitRecord()
        {
            RemitAdminReviewLog = new HashSet<RemitAdminReviewLog>();
        }

        public long Id { get; set; }
        public long UserId { get; set; }
        public string ArcName { get; set; }
        public string ArcNo { get; set; }
        public byte PayeeType { get; set; }
        public string IdImageA { get; set; }
        public string IdImageB { get; set; }
        public string IdImageC { get; set; }
        public string RealTimePic { get; set; }
        public string ESignature { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public long? FromCurrencyId { get; set; }
        public long? ToCurrencyId { get; set; }
        public double FromAmount { get; set; }
        public double ApplyExchangeRate { get; set; }
        public double TransactionExchangeRate { get; set; }
        public double Fee { get; set; }
        public byte FeeType { get; set; }
        public long? DiscountId { get; set; }
        public double? DiscountAmount { get; set; }
        public long? BeneficiaryId { get; set; }
        public short TransactionStatus { get; set; }
        public DateTime? PaymentTime { get; set; }
        public string PaymentCode { get; set; }
        public long? ArcScanRecordId { get; set; }
        public long? AmlScanRecordId { get; set; }
        public DateTime? FormalApplyTime { get; set; }

        public virtual ArcScanRecord ArcScanRecord { get; set; }
        public virtual OftenBeneficiary Beneficiary { get; set; }
        public virtual CurrencyCode ToCurrency { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<RemitAdminReviewLog> RemitAdminReviewLog { get; set; }
    }
}
