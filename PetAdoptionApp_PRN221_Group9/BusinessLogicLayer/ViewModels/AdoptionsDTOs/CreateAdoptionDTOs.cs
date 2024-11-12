using BusinessObjects.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.ViewModels.AdoptionsDTOs
{
    public class CreateAdoptionDTOs
    {
        
        public string? AdoptionReason { get; set; }
        public string? PetExperience { get; set; }
        public string? Address { get; set; }
        public string? ContactNumber { get; set; }
        public string? Notes { get; set; }
        public string? UserEmail { get; set; }
    }
}
