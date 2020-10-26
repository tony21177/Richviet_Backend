using System;
using System.Collections.Generic;

namespace Richviet.Services.Models
{
    public partial class UserLoginLog
    {
        public int Id { get; set; }
        public string Ip { get; set; }
        public string Address { get; set; }
        public byte LoginType { get; set; }
        public DateTimeOffset? LoginTime { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}
