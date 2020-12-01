using System;
using System.Collections.Generic;

namespace Frontend.DB.EF.Models
{
    public partial class User
    {
        public User()
        {
            Discount = new HashSet<Discount>();
            OftenBeneficiary = new HashSet<OftenBeneficiary>();
            RemitRecord = new HashSet<RemitRecord>();
            UserRegisterType = new HashSet<UserRegisterType>();
        }

        public long Id { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public byte Gender { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public DateTime? Birthday { get; set; }
        public byte Level { get; set; }

        public virtual PushNotificationSetting PushNotificationSetting { get; set; }
        public virtual UserArc UserArc { get; set; }
        public virtual ICollection<Discount> Discount { get; set; }
        public virtual ICollection<OftenBeneficiary> OftenBeneficiary { get; set; }
        public virtual ICollection<RemitRecord> RemitRecord { get; set; }
        public virtual ICollection<UserRegisterType> UserRegisterType { get; set; }
    }
}
