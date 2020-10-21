using System;
using System.Collections.Generic;

namespace Richviet.Services.Models
{
    public partial class User
    {
        public User()
        {
            OftenBeneficiar = new HashSet<OftenBeneficiar>();
            RemitRecord = new HashSet<RemitRecord>();
            UserLoginLog = new HashSet<UserLoginLog>();
        }

        public int Id { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTimeOffset? CreateTime { get; set; }
        public DateTimeOffset? UpdateTime { get; set; }

        public virtual ICollection<OftenBeneficiar> OftenBeneficiar { get; set; }
        public virtual ICollection<RemitRecord> RemitRecord { get; set; }
        public virtual ICollection<UserLoginLog> UserLoginLog { get; set; }
    }
}
