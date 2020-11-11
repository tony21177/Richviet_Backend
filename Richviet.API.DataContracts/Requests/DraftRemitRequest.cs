using Richviet.API.DataContracts.Validation;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Richviet.API.DataContracts.Requests
{
    [SwaggerSchema(Required = new[] { "Description" })]

    public class DraftRemitRequest
    {

        [CountryValidation("TW")]
        [SwaggerSchema("匯款國家")]
        public string? Country { get; set; }
        [SwaggerSchema("匯款金額")]
        public int? FromAmount { get; set; }
        [SwaggerSchema("常用收款人pk,對應/user/beneficiars回傳的pk")]
        public int? BeneficiarId { get; set; }
        [SwaggerSchema("大頭照檔名,填上經由/uploadPicture上傳回覆的檔名")]
        public string? PhotoFilename { get; set; }
        [SwaggerSchema("簽名檔名,填上經由/uploadPicture上傳回覆的檔名")]
        public string? SignatureFilename { get; set; }
        [SwaggerSchema("使用折扣券pk,對應/user/discount回傳的pk")]
        public int? DiscountId { get; set; }
        [SwaggerSchema("收款的幣別id,對應/currency/VN回傳的幣別PK")]
        public int? ToCurrencyId { get; set; }
    }
}
