using BusinessLogicLayer.ViewModels.PetImageDTOs;
using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.ViewModels.PetDTOs
{
    public class GetPetDTOs
    {
        public Guid Id { get; set; }
        public string? PetName { get; set; }
        public string? Age { get; set; }
        public string? Breed { get; set; }
        public string? Gender { get; set; }
        public string? Description { get; set; }
        public DateTime? RescuedDate { get; set; }
        
        //Show Shelters
        public Guid? ShelterId { get; set; }
        public string? ShelterName { get; set; }
        public List<PetImagesDTOs>? PetImages { get; set; }

    }
}
