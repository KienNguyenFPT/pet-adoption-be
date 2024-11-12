using BusinessObjects.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class Healths : BaseEntity
    {
        public string? ShortDescription { get; set; }
        public VaccineStatus? VaccineStatus { get; set; }
        public DateTime? Date {  get; set; }
        public Guid? PetId {  get; set; }
        public virtual Pet? Pet { get; set; }
    
    }
}
