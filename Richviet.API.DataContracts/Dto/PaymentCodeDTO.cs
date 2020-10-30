using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Richviet.Services.Models
{
    [SwaggerSchema(Required = new[] { "Description" })]
    public class PaymentCodeDTO
    {
        [SwaggerSchema("繳款序號字串,由前端gen QRcode")]
        public string Code { get; set; }
    }
}
