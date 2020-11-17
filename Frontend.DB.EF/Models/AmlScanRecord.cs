using System;
using System.Collections.Generic;

namespace Frontend.DB.EF.Models
{
    public partial class AmlScanRecord
    {
        public int Id { get; set; }
        public short AmlStatus { get; set; }
        public DateTime ScanTime { get; set; }
        public string Description { get; set; }
        public byte Event { get; set; }
    }
}
