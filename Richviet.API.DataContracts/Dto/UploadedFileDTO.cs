using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Richviet.API.DataContracts.Dto
{
    [SwaggerSchema(Required = new[] { "Description" })]
    public class UploadedFileDTO
    {
        [SwaggerSchema("檔名,格式為{type}_{ARC號碼}_{上傳時間timestamp}")]
        public string FileName { set; get; }
    }
}
