using System;
using System.Collections.Generic;

namespace Frontend.DB.EF.Models
{
    public partial class PushNotificationSetting
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string MobileToken { get; set; }
        public byte IsTurnOn { get; set; }

        public virtual User User { get; set; }
    }
}
