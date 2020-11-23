using System;
using System.Collections.Generic;
using System.Text;

namespace Users.Domains.Users.Command.Request
{
    public class UserLevelModifyVoRequest
    {
        public long Id { get; set; }

        public byte Level { get; set; }
    }
}
