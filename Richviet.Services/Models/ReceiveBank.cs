using System;
using System.Collections.Generic;

namespace Richviet.Services.Models
{
    public partial class ReceiveBank
    {
        public int Id { get; set; }
        public string SwiftCode { get; set; }
        public string Code { get; set; }
        public string VietName { get; set; }
        public string EnName { get; set; }
        public string TwName { get; set; }
        public int? SortNum { get; set; }
    }
}
