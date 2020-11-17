using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Richviet.API.DataContracts.Dto
{
    [SwaggerSchema(Required = new[] { "Description" })]
    public class NotificationDTO
    {
        [SwaggerSchema("使用者id")]
        public int UserId { get; set; }
        [SwaggerSchema("使用者通知狀態，1 = 開啟，0 = 關閉")]
        public byte IsTurnOn { get; set; }
    }
}
