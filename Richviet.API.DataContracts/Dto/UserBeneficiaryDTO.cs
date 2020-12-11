using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Richviet.API.DataContracts.Dto
{
    [SwaggerSchema(Required = new[] { "Description" })]
    public class UserBeneficiaryDTO
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
        [SwaggerSchema("收款銀行id,可以由/banks取得")]
        public int? ReceiveBankId { get; set; }
        [SwaggerSchema("收款銀行名-越南名稱")]
        public string VietName { get; set; }
        [SwaggerSchema("收款銀行名-英文名稱")]
        public string EnName { get; set; }
        [SwaggerSchema("收款銀行名-台灣名稱")]
        public string TwName { get; set; }

        [SwaggerSchema("會員id")]
        public int UserId { get; set; }
        [SwaggerSchema("與收款人關係id,對應/relations回傳的pk")]
        public int PayeeRelationId { get; set; }

        [SwaggerSchema("收款人關係0:父母,1:兄弟,2:子女")]
        public byte PayeeRelationType { get; set; }
        [SwaggerSchema("收款人關係描述")]
        public string PayeeRelationDescription { get; set; }
        [SwaggerSchema("收款方式,0:銀行")]
        public byte PayeeType { get; set; }


        public DateTimeOffset? CreateTime { get; set; }
        public DateTimeOffset? UpdateTime { get; set; }
    }
}
