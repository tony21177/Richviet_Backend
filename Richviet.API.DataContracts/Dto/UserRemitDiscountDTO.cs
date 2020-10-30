using Richviet.API.DataContracts.Converter;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Text.Json.Serialization;

namespace Richviet.API.DataContracts.Dto
{
    [SwaggerSchema(Required = new[] { "Description" })]
    public class UserRemitDiscountDTO
    {
        [SwaggerSchema("主鍵")]
        public int Id { get; set; }
        [SwaggerSchema("使用者id")]
        public int UserId { get; set; }
        [SwaggerSchema("折價金額")]
        public double Value { get; set; }
        [SwaggerSchema("開始生效日")]
        [JsonConverter(typeof(CustomDateConverter))]
        public DateTime? EffectiveDate { get; set; }
        [SwaggerSchema("失效日期")]
        [JsonConverter(typeof(CustomDateConverter))]
        public DateTime? ExpireDate { get; set; }
        [SwaggerSchema("使用狀態0:可使用,1:已使用,2:無效")]
        public byte UseStatus { get; set; }
    }
}
