using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Richviet.API.DataContracts.Requests
{
    public class TokenResource
    {
        public string Token { get; set; }
        public long Expiry { get; set; }
    }
}
