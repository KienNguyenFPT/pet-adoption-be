using BusinessObjects.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.ViewModels.AdoptionsDTOs
{
    public class AdoptionDTOs
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
        
        //Add Pet
        public Guid? PetId {  get; set; }
    }
}
