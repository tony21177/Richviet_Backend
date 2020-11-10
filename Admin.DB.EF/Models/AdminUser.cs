using System;
using System.Collections.Generic;

namespace Admin.DB.EF.Models
{
    public partial class AdminUser
    {
        public AdminUser()
        {
            UsersXroles = new HashSet<UsersXroles>();
        }

        public string UserCode { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public bool? IsSystemPassword { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string CellPhone { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public bool TwoFactorEnable { get; set; }
        public int TwoFactorType { get; set; }
        public bool IsActive { get; set; }
        public string Language { get; set; }
        public string TimeZone { get; set; }
        public string CreateUser { get; set; }
        public string UpdateUser { get; set; }
        public DateTime CreatTime { get; set; }
        public DateTime UpdateTime { get; set; }

        public virtual ICollection<UsersXroles> UsersXroles { get; set; }
    }
}
