using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Richviet.API.DataContracts.Dto
{
    [SwaggerSchema(Required = new[] { "Description" })]
    public class NotificationMessageDTO
    {
        [SwaggerSchema("訊息id")]
        public int Id { get; set; }
        [SwaggerSchema("使用者id")]
        public int UserId { get; set; }
        [SwaggerSchema("訊息標題")]
        public string Title { get; set; }
        [SwaggerSchema("訊息內容")]
        public string Content { get; set; }
        [SwaggerSchema("訊息圖片連結")]
        public string ImageUrl { get; set; }
        [SwaggerSchema("讀取狀態")]
        public bool IsRead { get; set; }
        [SwaggerSchema("訊息語言")]
        public string Language { get; set; }
        [SwaggerSchema("訊息發送時間")]
        public DateTime UpdateTime { get; set; }
    }
}
