using BusinessObjects.Enum;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class User : BaseEntity
    {
        public string? EmailAddress { get; set; }
        public string? PasswordHash { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber {  get; set; }
        public Role Role { get; set; }
        public string? ConfirmationToken { get; set; }
        public bool IsConfirmed { get; set; }
        



        //Relationship

        public virtual IEnumerable<Enrollment>? Enrollments { get; set; }
        public virtual IEnumerable<Donation>? Donations { get; set; }
        public virtual IEnumerable<Adoption>? Adoptions {  get; set; }
        
    }
}
