using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Richviet.API.DataContracts.Requests
{
    [SwaggerSchema(Required = new[] { "Description" })]
    public class NotificationSettingRequest
    {
        [SwaggerSchema("用於Firebase message識別裝置的token")]
        public string MobileToken { get; set; }

        [SwaggerSchema("是否關閉通知")]
        public bool IsTurnOn { get; set; }
    }
}
