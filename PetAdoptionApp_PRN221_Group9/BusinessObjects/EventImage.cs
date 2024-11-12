using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class EventImage : BaseEntity
    {
        public string? ImageUrl { get; set; }

        //Relationship
        public Guid? EventId {  get; set; }
        public virtual Event? Event { get; set; }
    }
}
