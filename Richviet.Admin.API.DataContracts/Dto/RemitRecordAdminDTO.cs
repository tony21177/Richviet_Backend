using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Richviet.Admin.API.DataContracts.Dto
{
    [SwaggerSchema(Required = new[] { "Description" })]
    public  class RemitRecordAdminDTO
    {
        [SwaggerSchema("交易紀錄主鍵")]
        public int Id { get; set; }
        [SwaggerSchema("交易單號")]
        public string OrderNo { get; set; }
        [SwaggerSchema("會員Id")]
        public int UserId { get; set; }
        [SwaggerSchema("提出草稿匯款申請時間")]
        public long CreateTime { get; set; }
        [SwaggerSchema("最後更改匯款時間")]
        public long UpdateTime { get; set; }
        [SwaggerSchema("正式匯款申請時間")]
        public long FormalApplyTime { get; set; }
        [SwaggerSchema("收款幣別")]
        public string ToCurrency { get; set; }
        [SwaggerSchema("會員名")]
        public string ArcName { get; set; }
        [SwaggerSchema("會員ArcNo")]
        public string ArcNo { get; set; }
        [SwaggerSchema("饋款金額")]
        public double FromAmount { get; set; }
        [SwaggerSchema("實際收款金額")]
        public double ToAmount { get; set; }
        [SwaggerSchema("申請時當下匯率")]
        public double ApplyExchangeRate { get; set; }
        [SwaggerSchema("實際匯款匯率")]
        public double TransactionExchangeRate { get; set; }
        [SwaggerSchema("手續費")]
        public double Fee { get; set; }
        [SwaggerSchema("折扣金額")]
        public double? DiscountAmount { get; set; }
        [SwaggerSchema("收款人名稱")]
        public string PayeeName { get; set; }
        [SwaggerSchema("收款銀行")]
        public string Bank { get; set; }
        [SwaggerSchema("收款人銀行帳號")]
        public string PayeeAddress { get; set; }
        [SwaggerSchema("收款人關係0:父母,1:兄弟,2:子女")]
        public byte PayeeRelationType { get; set; }
        [SwaggerSchema("收款人關係0:父母,1:兄弟,2:子女")]
        public string PayeeRelationTypeDescription { get; set; }
        [SwaggerSchema("-10:其他錯誤,-9: 審核失敗,-8: AML未通過,-7:交易逾期,0:草稿,1: 待ARC審核,2ARC審核成功,3:AML審核成功,4:營運人員確認OK,待會員繳款狀態,5: 已繳款,待營運人員處理,9:處理完成")]
        public short TransactionStatus { get; set; }
        [SwaggerSchema("繳款碼")]
        public string [] PaymentCode { get; set; }
    }
}
