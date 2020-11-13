using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Richviet.Admin.API.DataContracts.Dto
{
    [SwaggerSchema(Required = new[] { "Description" })]
    public class UserAdminListDTO
    {
        [SwaggerSchema("使用者 id")]
        public int Id { get; set; }
        [SwaggerSchema("使用者名稱")]
        public string Name { get; set; }
        [SwaggerSchema("ARC號碼")]
        public string ArcNo { get; set; }
        [SwaggerSchema("會員狀態")]
        public byte? KycStatus { get; set; }
        [SwaggerSchema("會員等級")]
        public byte Level { get; set; }
        [SwaggerSchema("註冊時間")]
        public DateTimeOffset? RegisterTime { get; set; }
    }
}
