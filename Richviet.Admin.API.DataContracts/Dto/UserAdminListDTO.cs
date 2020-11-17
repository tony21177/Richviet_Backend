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
        [SwaggerSchema("KYC審核狀態, -10:禁用,-9:KYC未通過,-8:AML未通過 ,0:草稿會員,1:待審核(註冊完),2:ARC驗證成功,3:AML通過,4:正式會員(KYC審核通過)")]
        public short? KycStatus { get; set; }
        [SwaggerSchema("會員等級")]
        public byte Level { get; set; }
        [SwaggerSchema("註冊時間")]
        public DateTimeOffset? RegisterTime { get; set; }
    }
}
