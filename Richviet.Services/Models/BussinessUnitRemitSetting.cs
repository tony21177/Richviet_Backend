using System;
using System.Collections.Generic;

namespace Richviet.Services.Models
{
    public partial class BussinessUnitRemitSetting
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public double RemitMin { get; set; }
        public double RemitMax { get; set; }
    }
}
