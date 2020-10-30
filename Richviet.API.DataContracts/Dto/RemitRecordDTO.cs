using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Richviet.Services.Models
{
    [SwaggerSchema(Required = new[] { "Description" })]
    public  class RemitRecordDTO
    {
        [SwaggerSchema("交易紀錄主鍵")]
        public int Id { get; set; }
        [SwaggerSchema("提出匯款申請時間")]
        public DateTimeOffset? CreateTime { get; set; }
        [SwaggerSchema("收款幣別")]
        public string ToCurrency { get; set; }
        [SwaggerSchema("饋款金額")]
        public double FromAmount { get; set; }
        [SwaggerSchema("實際收款金額")]
        public double ToAmount { get; set; }
        [SwaggerSchema("匯率")]
        public double TransactionExchangeRate { get; set; }
        [SwaggerSchema("手續費")]
        public double Fee { get; set; }
        [SwaggerSchema("折扣金額")]
        public double? DiscountAmount { get; set; }
        [SwaggerSchema("收款人名稱")]
        public string Name { get; set; }
        [SwaggerSchema("收款人銀行帳號")]
        public string PayeeAddress { get; set; }
        [SwaggerSchema("收款人關係0:父母,1:兄弟,2:子女")]
        public byte Type { get; set; }
        [SwaggerSchema("交易狀態-9:其他錯誤,-1: 審核失敗,0: 待審核(系統進入arc_status流程),1: 待繳款,2: 已繳款,3:處理完成")]
        public byte TransactionStatus { get; set; }
    }
}
