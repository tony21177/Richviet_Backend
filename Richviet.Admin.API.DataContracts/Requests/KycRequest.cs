using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Richviet.Admin.API.DataContracts.Requests
{
    [SwaggerSchema(Required = new[] { "Description" })]
    public class KycRequest
    {
       
        [Required]
        [SwaggerSchema("KYC審核狀態, -10:禁用,-9:KYC未通過,-8:AML未通過 ,0:草稿會員,1:待審核(註冊完),2:ARC驗證成功,3:AML通過,9:正式會員(KYC審核通過)")]
        public short KycStatus { get; set; }
    }
}
