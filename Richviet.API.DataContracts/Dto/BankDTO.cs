using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Richviet.API.DataContracts.Dto
{
    [SwaggerSchema(Required = new[] { "Description" })]
    public class BankDTO
    {
        [SwaggerSchema("可收款銀行id")]
        public int Id { get; set; }
        [SwaggerSchema("可收款銀行swift code")]
        public string SwiftCode { get; set; }
        [SwaggerSchema("可收款銀行代碼")]
        public string Code { get; set; }
        [SwaggerSchema("可收款銀行越南名稱")]
        public string VietName { get; set; }
        [SwaggerSchema("可收款銀行英文名稱")]
        public string EnName { get; set; }
        [SwaggerSchema("可收款銀行中文名稱")]
        public string TwName { get; set; }
    }
}
