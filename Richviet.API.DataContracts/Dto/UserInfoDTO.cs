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
        [SwaggerSchema("0:草稿會員,1:正式會員")]
        public byte Status { get; set; }
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
        [SwaggerSchema("0:未認證,1:待審核,2:審核通過,9:未通過")]
        public byte? KycStatus { get; set; }
        public DateTimeOffset? KycStatusUpdateTime { get; set; }
        public DateTimeOffset? RegisterTime { get; set; }
        [SwaggerSchema("第三方登入平台ID")]
        public string AuthPlatformId { get; set; }
        [SwaggerSchema("註冊方式,0:平台本身,1:FB,2:apple,3:google,4:zalo")]
        public byte RegisterType { get; set; }
        [SwaggerSchema("第三方登入平台email")]
        public string LoginPlatformEmal { get; set; }
        [SwaggerSchema("第三方登入平台name")]
        public string Name { get; set; }
    }
}
