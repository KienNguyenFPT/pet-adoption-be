using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class Pet : BaseEntity
    {
        public string? PetName {  get; set; }
        public string? Age { get; set; }
        public string? Breed { get; set; }
        public string? Gender { get; set; }
        public string? Description { get; set; }
        public DateTime? RescuedDate { get; set; }

        //Relationship
        /*public Guid? AdoptionId { get; set; }
        public virtual Adoption? Adoption { get; set;}*/
        public Guid? ShelterId {  get; set; }
        public virtual Shelter? Shelter { get; set; }
        
        public virtual IEnumerable<PetImage>? PetImages {  get; set; }
        public virtual IEnumerable<Healths>? Healths { get; set; }
        public virtual IEnumerable<Adoption>? Adoptions { get; set; }
    }
}
