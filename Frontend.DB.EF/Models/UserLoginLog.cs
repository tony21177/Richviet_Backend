using System;
using System.Collections.Generic;

namespace Frontend.DB.EF.Models
{
    public partial class UserLoginLog
    {
        public long Id { get; set; }
        public string Ip { get; set; }
        public string Address { get; set; }
        public byte LoginType { get; set; }
        public DateTime? LoginTime { get; set; }
        public long UserId { get; set; }

        public virtual User User { get; set; }
    }
}
