using System;
using System.Collections.Generic;

namespace Admin.DB.EF.Models
{
    public partial class Roles
    {
        public Roles()
        {
            UsersXroles = new HashSet<UsersXroles>();
        }

        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public string CreateUser { get; set; }
        public string UpdateUser { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }

        public virtual ICollection<UsersXroles> UsersXroles { get; set; }
    }
}
