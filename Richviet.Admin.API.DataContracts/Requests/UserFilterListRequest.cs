using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Richviet.Admin.API.DataContracts.Requests
{
    [SwaggerSchema(Required = new[] { "Description" })]
    public class UserFilterListRequest
    {
        [SwaggerSchema("使用者名稱")]
        public string Name { get; set; } = string.Empty;
        [SwaggerSchema("ARC號碼")]
        public string ArcNo { get; set; } = string.Empty;
        [SwaggerSchema("會員狀態 正式")]
        public bool KycFormal { get; set; } = false;
        [SwaggerSchema("會員狀態 審核中")]
        public bool KycUnderReview { get; set; } = false;
        [SwaggerSchema("會員狀態 草稿")]
        public bool KycDraft { get; set; } = false;
        [SwaggerSchema("會員狀態 已停用")]
        public bool KycDisabled { get; set; } = false;
        [SwaggerSchema("會員等級 VIP")]
        public bool LevelVIP { get; set; } = false;
        [SwaggerSchema("會員等級 一般會員")]
        public bool LevelNormal { get; set; } = false;
        [SwaggerSchema("會員等級 高風險")]
        public bool LevelRisk { get; set; } = false;
        [SwaggerSchema("搜索註冊開始時間")]
        public long RegisterStartTime { get; set; } = 0;
        [SwaggerSchema("搜索註冊結束時間")]
        public long RegisterEndTime { get; set; } = DateTime.MaxValue.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Ticks;
    }
}
