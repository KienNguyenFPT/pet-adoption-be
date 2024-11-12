using BusinessObjects.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class Adoption : BaseEntity
    {
        public DateTime? ApplicationDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public AdoptionStatus AdoptionStatus { get; set; }
        public string? AdoptionReason { get; set; }
        public string? PetExperience { get; set; }
        public string? Address { get; set; }
        public string? ContactNumber { get; set; }
        public string? Notes { get; set; }
        public string? UserEmail { get; set; }


        //Relationship
        public Guid? UserId {  get; set;}
        public virtual User? User {  get; set;}
        /*public virtual IEnumerable<Pet>? Pets { get; set;}*/
        public Guid? PetId {  get; set; }
        public virtual Pet? Pet { get; set;}
    }
}
