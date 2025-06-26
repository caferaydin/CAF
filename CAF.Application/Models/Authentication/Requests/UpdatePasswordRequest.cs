using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAF.Application.Models.Authentication.Requests
{
    public class UpdatePasswordRequest
    {
        public string UserId { get; set; }
        public string ResetToken { get; set; }
        public string NewPassword { get; set; }
    }
}
