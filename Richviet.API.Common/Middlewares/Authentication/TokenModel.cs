using System;
using System.Collections.Generic;
using System.Text;

namespace Richviet.API.Common.Middlewares.Authentication
{
    class TokenModel
    {
        public string id { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public long iat { get; set; }
        public long exp { get; set; }
        public long nbf { get; set; }
        public string country { get; set; }


    }
}
