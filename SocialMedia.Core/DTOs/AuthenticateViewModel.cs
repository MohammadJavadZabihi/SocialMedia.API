using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.DTOs
{
    public class AuthenticateRequestBodyViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
