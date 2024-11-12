using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class Shelter : BaseEntity
    {
        public string? ShelterName { get; set; }
        public string? Address { get; set; }
        public string? Description {  get; set; }
        public int? LimitedCapacity { get; set; }
        public int? CurrentCapacity {  get; set; }

        //Relationship
        public virtual IEnumerable<Pet>? Pets { get; set; }
        public virtual IEnumerable<Donation>? Donations { get; set; }
    }
}
