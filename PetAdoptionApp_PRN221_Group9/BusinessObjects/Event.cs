using BusinessObjects.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class Event : BaseEntity
    {
        public DateTime? StartDate {  get; set; }
        public DateTime? EndDate { get; set;}
     
        public string? EventName {  get; set; }
        public string? Description {  get; set; }
        public EventType? EventType { get; set; }
        public EventStatus? EventStatus { get; set; }
        public string? Location { get; set; }
        public string? Sponsors { get; set; }
        public int? LimitedCapacity { get; set; }

        //Relationship 
        public virtual IEnumerable<Enrollment>? Enrollments { get; set; }
        public virtual IEnumerable<EventImage>? Images { get; set; }
    }
}
