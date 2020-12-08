using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Richviet.Background.Service.Base
{
    public class Config
    {
        //public static readonly string DB_CONN = "Server=LAPTOP-DCDEKIUG;Database=General;Trusted_Connection=True;";
        public static readonly string DB_CONN = "Data Source=.\\SQLEXPRESS;Database=General;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False";

        public static readonly int SLEEP_TIME = 30 * 1000;
    }
}
