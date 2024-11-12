using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class Enrollment : BaseEntity
    {
        public Guid? UserId { get; set; }
        public Guid? EventId { get; set; }
        public virtual Event? Event { get; set; }
        public virtual User? User { get; set; }
    }
}
