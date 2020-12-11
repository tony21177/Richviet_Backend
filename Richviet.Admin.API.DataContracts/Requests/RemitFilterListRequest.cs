using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text;

namespace Richviet.Admin.API.DataContracts.Requests
{
    [SwaggerSchema(Required = new[] { "Description" })]
    public class RemitFilterListRequest
    {
        [SwaggerSchema("交易紀錄主鍵")]
        public int? Id { get; set; }
        [SwaggerSchema("會員名")]
        public string ArcName { get; set; } = string.Empty;
        [SwaggerSchema("會員ArcNo")]
        public string ArcNo { get; set; } = string.Empty;
        [SwaggerSchema("收款銀行")]
        public string Bank { get; set; } = string.Empty;
        [SwaggerSchema("過濾匯款開始金額")]
        public double StartAmount { get; set; } = 0;
        [SwaggerSchema("過濾匯款結束金額")]
        public double EndAmount { get; set; } = double.MaxValue;



        [SwaggerSchema("1:待KYC審核")]
        public bool KycReviewStatus { get; set; } = false;
        [SwaggerSchema("2:待AML審核")]
        public bool AmlReviewStatus { get; set; } = false;
        [SwaggerSchema("3:待營運人員審核")]
        public bool StaffReviewStatus { get; set; } = false;
        [SwaggerSchema("4:待繳費")]
        public bool PendingPayStatus { get; set; } = false;
        [SwaggerSchema("5:待匯款")]
        public bool PendingRemitStatus { get; set; } = false;
        [SwaggerSchema("9:交易完成")]
        public bool FinishStatus { get; set; } = false;
        [SwaggerSchema("-7:交易逾期")]
        public bool OverdueStatus { get; set; } = false;
        [SwaggerSchema("-10:其他錯誤,-9: 審核失敗,-8: AML未通過")]
        public bool FailStatus { get; set; } = false;


        [SwaggerSchema("過濾開始時間")]
        public long CreateStartTime { get; set; } = 0;
        [SwaggerSchema("過濾結束時間")]
        public long CreateEndTime { get; set; } = DateTime.MaxValue.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Ticks;
    }
}
