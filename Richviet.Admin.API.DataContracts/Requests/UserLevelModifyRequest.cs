using System;
using System.Collections.Generic;
using System.Text;

namespace Richviet.Admin.API.DataContracts.Requests
{
    public class UserLevelModifyRequest
    {
        public long Id { get; set; }

        public byte Level { get; set; }
    }
}
