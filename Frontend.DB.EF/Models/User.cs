﻿using System;
using System.Collections.Generic;

namespace Frontend.DB.EF.Models
{
    public partial class User
    {
        public User()
        {
            Discount = new HashSet<Discount>();
            OftenBeneficiar = new HashSet<OftenBeneficiar>();
            RemitRecord = new HashSet<RemitRecord>();
            UserLoginLog = new HashSet<UserLoginLog>();
            UserRegisterType = new HashSet<UserRegisterType>();
        }

        public long Id { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Gender { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public DateTime? Birthday { get; set; }
        public int Status { get; set; }

        public virtual UserArc UserArc { get; set; }
        public virtual ICollection<Discount> Discount { get; set; }
        public virtual ICollection<OftenBeneficiar> OftenBeneficiar { get; set; }
        public virtual ICollection<RemitRecord> RemitRecord { get; set; }
        public virtual ICollection<UserLoginLog> UserLoginLog { get; set; }
        public virtual ICollection<UserRegisterType> UserRegisterType { get; set; }
    }
}