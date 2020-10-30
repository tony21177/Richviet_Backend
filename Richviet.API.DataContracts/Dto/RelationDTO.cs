using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Richviet.Services.Models
{
    [SwaggerSchema(Required = new[] { "Description" })]
    public class RelationDTO
    {
        [SwaggerSchema("id")]
        public int Id { get; set; }
        [SwaggerSchema("0,1,2....")]
        public byte Type { get; set; }
        [SwaggerSchema("關係描述")]
        public string Description { get; set; }
    }
}
