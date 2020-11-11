using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Richviet.Admin.API.DataContracts.Dto
{
    [SwaggerSchema(Required = new[] { "Description" })]
    public class EditRemitSettingDTO
    {
        [SwaggerSchema("匯款設定id")]
        public int Id { get; set; }
        [SwaggerSchema("服務所在國家")]
        public string Country { get; set; }
        [SwaggerSchema("匯款金額允許最低")]
        public double RemitMin { get; set; }
        [SwaggerSchema("匯款金額允許最高")]
        public double RemitMax { get; set; }
    }
}
