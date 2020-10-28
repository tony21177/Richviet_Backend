using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Richviet.Services.Models
{
    [SwaggerSchema(Required = new[] { "Description" })]
    public class UserBeneficiarsDTO
    {
        [SwaggerSchema("主鍵")]
        public int Id { get; set; }
        [SwaggerSchema("收款人名稱")]
        public string Name { get; set; }
        [SwaggerSchema("收款人銀行帳號")]
        public string PayeeAddress { get; set; }
        [SwaggerSchema("收款人ID,選填")]
        public string PayeeId { get; set; }
        [SwaggerSchema("收款人備註")]
        public string Note { get; set; }
        [SwaggerSchema("收款銀行名-越南名稱")]
        public string VietName { get; set; }
        [SwaggerSchema("收款銀行名-英文名稱")]
        public string EnName { get; set; }
        [SwaggerSchema("收款銀行名-台灣名稱")]
        public string TwName { get; set; }

        [SwaggerSchema("收款人帳號(arc_no)")]
        public string ArcNo { get; set; }
        [SwaggerSchema("收款人關係0:父母,1:兄弟,2:子女")]
        public byte Type { get; set; }
        public DateTimeOffset? CreateTime { get; set; }
        public DateTimeOffset? UpdateTime { get; set; }
    }
}
