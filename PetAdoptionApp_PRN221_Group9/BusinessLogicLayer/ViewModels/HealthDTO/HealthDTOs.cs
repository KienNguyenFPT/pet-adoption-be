using BusinessObjects.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.ViewModels.HealthDTO
{
    public class HealthDTOs
    {
        public Guid Id { get; set; }
        public string? ShortDescription { get; set; }
        public VaccineStatus? VaccineStatus { get; set; }
        public DateTime? Date { get; set; }
        public Guid? PetId { get; set; }
    }
}
