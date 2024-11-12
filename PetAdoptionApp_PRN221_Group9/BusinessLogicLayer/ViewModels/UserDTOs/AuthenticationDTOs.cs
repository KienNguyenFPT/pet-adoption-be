using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.ViewModels.UserDTOs
{
    public class AuthenticationDTOs
    {
        public string EmailAddress { get; set; }
        public string PasswordHash { get; set; }
    }
}
