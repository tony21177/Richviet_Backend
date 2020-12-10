using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Entity
{
    public class Claims
    {
        public Claims()
        {
        }

        public Claims(string type, string value)
        {
            Type = type;
            Value = value;
        }

        [MaxLength(32)]
        public int ClaimsId { get; set; }

        [MaxLength(32)]
        public string Type { get; set; }

        [MaxLength(32)]
        public string Value { get; set; }

        public virtual User User { get; set; }
    }
}
