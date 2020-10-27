using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

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
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? EffectiveDate { get; set; }
        [SwaggerSchema("失效日期")]
        public DateTime? ExpireDate { get; set; }
    }
}
