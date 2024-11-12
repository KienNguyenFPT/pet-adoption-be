using BusinessObjects;
using BusinessObjects.Enum;

namespace BusinessLogicLayer.ViewModels.EventDTO {
    public class EventDTO {

        public Guid? Id { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string? EventName { get; set; }
        public string? Description { get; set; }
        public EventStatus? EventStatus { get; set; }
        public EventType? EventType { get; set; }
        public string? Location { get; set; }

        //Relationship 
        public virtual IEnumerable<EnrollmentDTO>? Enrollments { get; set; }
        public virtual IEnumerable<EventImageDTO>? Images { get; set; }
    }
}
