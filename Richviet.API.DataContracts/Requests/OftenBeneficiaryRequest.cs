using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Richviet.API.DataContracts.Requests
{
    [SwaggerSchema(Required = new[] { "Description" })]

    public class OftenBeneficiaryRequest
    {

        [Required]
        [SwaggerSchema("收款人名稱")]
        public string Name { get; set; }
        [Required]
        [SwaggerSchema("收款人銀行帳號")]
        public string PayeeAddress { get; set; }
        [SwaggerSchema("收款人id,選填")]
        public string PayeeId { get; set; }
        [SwaggerSchema("備註")]
        public string Note { get; set; }
        [Required]
        [SwaggerSchema("收款銀行id,可以由/banks取得")]
        public int? ReceiveBankId { get; set; }
        [Required]
        [SwaggerSchema("收款方式,0:銀行")]
        public int PayeeType { get; set; }
        //[JsonIgnore]
        //public int PayeeTypeId { get; set; }

        [SwaggerSchema("與收款人關係id,可以由/relations取得")]
        public int PayeeRelationId { get; set; }
    }
}
