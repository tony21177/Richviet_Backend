using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Richviet.API.DataContracts.Dto
{
    [SwaggerSchema(Required = new[] { "Description" })]
    public class RegisterResponseDTO
    {
        [SwaggerSchema("Bearer JWT前端request header要帶Authorization: Bearer <Jwt>")]
        public string Jwt { get; set; }
        [SwaggerSchema("KYC審核狀態, -10:禁用,-9:KYC未通過,-8:AML未通過 ,0:草稿會員,1:待審核(註冊完),2:ARC驗證成功,3:AML通過,4:正式會員(KYC審核通過)")]
        public short kycStatus { get; set; }
    }
}
