using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Richviet.Admin.API.DataContracts.Dto
{
    [SwaggerSchema(Required = new[] { "Description" })]
    public class EditRelationDTO
    {
        [SwaggerSchema("id")]
        public int Id { get; set; }
        [SwaggerSchema("0,1,2....")]
        public byte Type { get; set; }
        [SwaggerSchema("關係描述")]
        public string Description { get; set; }
    }
}
