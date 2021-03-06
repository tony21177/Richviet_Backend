﻿using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Richviet.API.DataContracts.Requests
{
    [SwaggerSchema(Required = new[] { "Description" })]
    public class NotificationSettingRequest
    {
        [SwaggerSchema("開關通知，開啟 true = 1，關閉 false = 0")]
        public bool IsTurnOn { get; set; }
    }
}
