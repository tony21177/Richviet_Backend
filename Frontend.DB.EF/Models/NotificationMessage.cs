using System;
using System.Collections.Generic;

namespace Frontend.DB.EF.Models
{
    public partial class NotificationMessage
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public bool IsRead { get; set; }
        public string Language { get; set; }
        public DateTime UpdateTime { get; set; }
        public DateTime CreateTime { get; set; }

        public virtual User User { get; set; }
    }
}
