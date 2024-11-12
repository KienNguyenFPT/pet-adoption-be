using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class PetImage : BaseEntity
    {
        public string? Image {  get; set; }

        //Relatiionship
        public Guid? PetId { get; set; }
        public virtual Pet? Pet { get; set; }
    }
}
