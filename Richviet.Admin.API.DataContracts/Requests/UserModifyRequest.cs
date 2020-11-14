using System;
using System.Collections.Generic;
using System.Text;
using Swashbuckle.AspNetCore.Annotations;

namespace Richviet.Admin.API.DataContracts.Requests
{
    public class UserModifyRequest
    {
        [SwaggerSchema("使用者唯一碼")]
        public long Id { get; set; }

        [SwaggerSchema("使用者電話")]
        public string Phone { get; set; }

        [SwaggerSchema("電子信箱")]
        public string Email { get; set; }

        [SwaggerSchema("性別")]
        public int Gender { get; set; }

        [SwaggerSchema("生日")]
        public DateTime? Birthday { get; set; }

        [SwaggerSchema("會員等級0:一般會員;1:VIP;9:高風險")]
        public byte Level { get; set; }

        [SwaggerSchema("國籍")]
        public string Country { get; set; }

        [SwaggerSchema("ARC姓名")]
        public string ArcName { get; set; }

        [SwaggerSchema("ARC編號")]
        public string ArcNo { get; set; }

        [SwaggerSchema("護照號碼")]
        public string PassportId { get; set; }

        [SwaggerSchema("背面序號")]
        public string BackSequence { get; set; }

        [SwaggerSchema("發證日期")]
        public DateTime? ArcIssueDate { get; set; }

        [SwaggerSchema("KYC審核狀態, 10:禁用,9:KYC未通過, 0:草稿會員,1:待審核(註冊完),2:正式會員(KYC審核通過);\\n")]
        public int? KycStatus { get; set; }


    }
}
