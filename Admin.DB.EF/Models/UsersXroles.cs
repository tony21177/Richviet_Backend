using System;
using System.Collections.Generic;

namespace Admin.DB.EF.Models
{
    public partial class UsersXroles
    {
        public string RoleCode { get; set; }
        public string UserCode { get; set; }

        public virtual Roles RoleCodeNavigation { get; set; }
        public virtual AdminUser UserCodeNavigation { get; set; }
    }
}
