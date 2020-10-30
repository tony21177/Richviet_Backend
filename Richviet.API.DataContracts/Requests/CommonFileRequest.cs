using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Richviet.API.DataContracts.Requests
{
    [SwaggerSchema(Required = new[] { "Description" })]
    public class CommonFileRequest
    {
        [Required]
        [SwaggerSchema("0:及時照,1:簽名照")]
        public byte ImageType { get; set; }
        [Required]
        public IFormFile Image { get; set; }


    }
}
