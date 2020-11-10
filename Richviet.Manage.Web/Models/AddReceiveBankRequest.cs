using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Richviet.Manage.Web.Models
{
    public class AddReceiveBankRequest
    {
        public string SwiftCode { get; set; }
        public string Code { get; set; }
        public string VietName { get; set; }
        public string EnName { get; set; }
        public string TwName { get; set; }
    }
}
