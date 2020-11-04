using Richviet.API.DataContracts.Converter;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Richviet.API.DataContracts.Requests
{
    [SwaggerSchema(Required = new[] { "Description" })]
    public class RegisterRequest
    {
        [Required]
        [SwaggerSchema("註冊真實姓名")]
        public string name { get; set; }
        [Required]
        [SwaggerSchema("性別 0:其他(包括未填)\\n1:男\\n2:女\\n")]
        public int gender { get; set; }
        [Required]
        [JsonConverter(typeof(CustomDateConverter))]
        [SwaggerSchema("生日,格式yyyy/mm/dd")]
        public DateTime? birthday { get; set; }
        [Required]
        [SwaggerSchema("手機號碼")]
        public string phone { get; set; }
        [Required]
        [SwaggerSchema("國籍")]
        public string country { get; set; }
        [Required]
        [SwaggerSchema("護照號碼")]
        public string passportNumber { get; set; }
        [Required]
        [SwaggerSchema("身分證統一編號")]
        public string personalID { get; set; }
        [Required]
        [JsonConverter(typeof(CustomDateConverter))]
        [SwaggerSchema("核發日期,格式yyyy/mm/dd")]
        public DateTime? issue { get; set; }
        [Required]
        [JsonConverter(typeof(CustomDateConverter))]
        [SwaggerSchema("居留期限,格式yyyy/mm/dd")]
        public DateTime? expiry { get; set; }
        [Required]
        [SwaggerSchema("背面序號")]
        public string backCode { get; set; }
        [Required]
        [SwaggerSchema("證件正面URL")]
        public string certificateA { get; set; }
        [Required]
        [SwaggerSchema("證件反面URL")]
        public string certificateB { get; set; }
    }
}
