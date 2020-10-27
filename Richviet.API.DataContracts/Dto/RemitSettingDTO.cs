using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Richviet.API.DataContracts.Dto
{
    [SwaggerSchema(Required = new[] { "Description" })]
    public class RemitSettingDTO
    {
        [SwaggerSchema("服務所在國家")]
        public string country { get; set; }
        [SwaggerSchema("匯款金額允許最低")]
        public double remitMin { get; set; }
        [SwaggerSchema("匯款金額允許最高")]
        public double remitMax { get; set; }
    }
}
