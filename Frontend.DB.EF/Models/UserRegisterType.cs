using System;
using System.Collections.Generic;

namespace Frontend.DB.EF.Models
{
    public partial class UserRegisterType
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Name { get; set; }
        public string AuthPlatformId { get; set; }
        public int RegisterType { get; set; }
        public string Email { get; set; }
        public DateTime? RegisterTime { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }

        public virtual User User { get; set; }
    }
}
