using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.ViewModels.PetDTOs
{
    public class PetDTOs
    {
        public string? PetName { get; set; }
        public string? Age { get; set; }
        public string? Breed { get; set; }
        public string? Gender { get; set; }
        public string? Description { get; set; }
        public DateTime? RescuedDate { get; set; }

        //add update Shelter
        public Guid? ShelterId { get; set; }
    }
}
