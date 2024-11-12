using BusinessObjects.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.ViewModels.UserDTOs
{
    public class UpdateUserDTO
    {
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public Role? Role { get; set; }
    }
}
