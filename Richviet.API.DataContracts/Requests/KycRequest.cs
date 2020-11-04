using Swashbuckle.AspNetCore.Annotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace Richviet.API.DataContracts.Requests
{
    [SwaggerSchema(Required = new[] { "Description" })]
    public class KycRequest
    {
        [Required]
        [SwaggerSchema("會員ID")]
        public int Id { get; set; }
        [Required]
        [SwaggerSchema("0:未認證,1:待審核,2:審核通過,9:未通過")]
        public byte kycStatus { get; set; }
    }
}
