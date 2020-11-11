using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Richviet.Admin.API.DataContracts.Requests
{
    [SwaggerSchema(Required = new[] { "Description" })]
    public class AddPayeeRelationTypeRequest
    {
        [SwaggerSchema("0,1,2....")]
        public byte Type { get; set; }
        [SwaggerSchema("關係描述")]
        public string Description { get; set; }
    }
}
