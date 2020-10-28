using System;
using System.Collections.Generic;

namespace Richviet.Services.Models
{
    public partial class UserRegisterType
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string AuthPlatformId { get; set; }
        public byte RegisterType { get; set; }
        public string Email { get; set; }
        public DateTimeOffset? RegisterTime { get; set; }
        public DateTimeOffset? CreateTime { get; set; }
        public DateTimeOffset? UpdateTime { get; set; }

        public virtual User User { get; set; }
    }
}
