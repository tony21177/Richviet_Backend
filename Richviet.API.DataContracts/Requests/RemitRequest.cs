using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Richviet.Services.Models
{
    [SwaggerSchema(Required = new[] { "Description" })]

    public class RemitRequest
    {

        [Required]
        [SwaggerSchema("匯款金額")]
        public double FromAmount { get; set; }
        [Required]
        [SwaggerSchema("常用收款人pk,對應/user/beneficiars回傳的pk")]
        public int BenefiaiarId { get; set; }
        [Required]
        [SwaggerSchema("大頭照檔名,填上經由/uploadPicture上傳回覆的檔名")]
        public string PhotoFilename { get; set; }
        [Required]
        [SwaggerSchema("簽名檔名,填上經由/uploadPicture上傳回覆的檔名")]
        public string SignatureFilename { get; set; }
        [SwaggerSchema("使用折扣券pk,對應/user/discount回傳的pk")]
        public int DiscountId { get; set; }
        [Required]
        [SwaggerSchema("收款的幣別id,對應/currency/VN回傳的幣別PK")]
        public int ToCurrencyId { get; set; }
    }
}
