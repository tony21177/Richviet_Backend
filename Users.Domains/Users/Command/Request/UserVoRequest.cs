using System;
using System.Collections.Generic;
using System.Text;

namespace Users.Domains.Users.Command.Request
{
    public class UserVoRequest
    {
        
        public long Id { get; set; }

       
        public string Phone { get; set; }

        
        public string Email { get; set; }

  
        public byte Gender { get; set; }

        
        public DateTime? Birthday { get; set; }


        public byte Level { get; set; }


        public string Country { get; set; }

   
        public string ArcName { get; set; }


        public string ArcNo { get; set; }


        public string PassportId { get; set; }


        public string BackSequence { get; set; }

      
        public DateTime? ArcIssueDate { get; set; }
        
        public sbyte? KycStatus { get; set; }
    }
}
