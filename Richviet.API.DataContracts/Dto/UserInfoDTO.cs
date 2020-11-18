using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Richviet.API.DataContracts.Dto

{
    [SwaggerSchema(Required = new[] { "Description" })]
    public partial class UserInfoDTO
    {
        public int Id { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        [SwaggerSchema("0:未填,1:男,2:女")]
        public byte Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public string Country { get; set; }
        public string ArcName { get; set; }
        public string ArcNo { get; set; }
        public string PassportId { get; set; }
        public string BackSequence { get; set; }
        [SwaggerSchema("證件正面")]
        public string IdImageA { get; set; }
        [SwaggerSchema("證件反面")]
        public string IdImageB { get; set; }
        [SwaggerSchema("手持證件")]
        public string IdImageC { get; set; }

        [SwaggerSchema("KYC審核狀態, -10:禁用,-9:KYC未通過,-8:AML未通過 ,0:草稿會員,1:待審核(註冊完),2:ARC驗證成功,3:AML通過,9:正式會員(KYC審核通過)")]
        public short KycStatus { get; set; }
        public long KycStatusUpdateTime { get; set; }
        public long RegisterTime { get; set; }
        [SwaggerSchema("第三方登入平台ID")]
        public string AuthPlatformId { get; set; }
        [SwaggerSchema("註冊方式,0:平台本身,1:FB,2:apple,3:google,4:zalo")]
        public byte RegisterType { get; set; }
        [SwaggerSchema("第三方登入平台email")]
        public string LoginPlatformEmal { get; set; }
        [SwaggerSchema("第三方登入平台name")]
        public string Name { get; set; }
        [SwaggerSchema("會員等級0:一般會員;1:VIP;9:高風險")]
        public byte Level { get; set; }
    }
}
