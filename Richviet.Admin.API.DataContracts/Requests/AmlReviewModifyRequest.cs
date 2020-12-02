using System;
using System.Collections.Generic;
using System.Text;
using Swashbuckle.AspNetCore.Annotations;

namespace Richviet.Admin.API.DataContracts.Requests
{
    public class AmlReviewModifyRequest
    {
        [SwaggerSchema("匯款交易識別碼")]
        public long RecordId { get; set; }

        [SwaggerSchema("AML審核結果")]
        public bool IsAmlPass { get; set; }


        [SwaggerSchema("備註")]
        public string Comment { get; set; }
    }
}
