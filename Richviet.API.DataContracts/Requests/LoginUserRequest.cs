using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Richviet.API.DataContracts.Requests
{
    public class LoginUserRequest
    {
        [Required]
        public string userId { get; set; }
        [Required]
        public string name { get; set; }
        public string email { get; set; }
        public string linkURL { get; set; }
        public long refreshDate { get; set; }
        public string imageURL { get; set; }
        public string gender { get; set; }
        [Required]
        public string accessToken { get; set; }
        [Required]
        public string loginType { get; set; }
        [Required]
        public string countryOfApp { get; set; }
        public TokenResource refreshToken { get; set; }
    }
}
