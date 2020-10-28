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
        public string accessToken { get; set; }
        [Required]
        public string permissions { get; set; }
        [Required]
        public int loginType { get; set; }
        [Required]
        public string countryOfApp { get; set; }
        public TokenResource refreshToken { get; set; }
    }
}
