using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Richviet.Admin.API.DataContracts.Dto
{
    [SwaggerSchema(Required = new[] { "Description" })]
    public class UserDetailDTO
    {
        [SwaggerSchema("使用者 id")]
        public int Id { get; set; }
        [SwaggerSchema("使用者名稱")]
        public string Name { get; set; }
        [SwaggerSchema("ARC號碼")]
        public string ArcNo { get; set; }
        [SwaggerSchema("KYC審核狀態, -10:禁用,-9:KYC未通過,-8:AML未通過 ,0:草稿會員,1:待審核(註冊完),2:ARC驗證成功,3:AML通過,9:正式會員(KYC審核通過)")]
        public short? KycStatus { get; set; }
        [SwaggerSchema("會員等級")]
        public byte Level { get; set; }
        [SwaggerSchema("性別 0:未填,1:男,2:女")]
        public byte Gender { get; set; }
        [SwaggerSchema("國家")]
        public string Country { get; set; }
        [SwaggerSchema("生日")]
        public DateTime? Birthday { get; set; }
        [SwaggerSchema("護照號碼")]
        public string PassportId { get; set; }
        [SwaggerSchema("核發日期")]
        public DateTime? ArcIssueDate { get; set; }
        [SwaggerSchema("居留期限")]
        public DateTime? ArcExpireDate { get; set; }
        [SwaggerSchema("背面序號")]
        public string BackSequence { get; set; }
        [SwaggerSchema("電話")]
        public string Phone { get; set; }
        [SwaggerSchema("最後上線時間")]
        public long LoginTime { get; set; }
        [SwaggerSchema("最近上線地點")]
        public string Address { get; set; }
        [SwaggerSchema("居留證正面")]
        public string IdImageA { get; set; }
        [SwaggerSchema("居留證反面")]
        public string IdImageB { get; set; }
    }
}
