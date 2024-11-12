using BusinessObjects;
using BusinessObjects.Enum;

namespace BusinessLogicLayer.ViewModels.EventDTO {
    public class BasicEventRequestDTO {
        public Guid? Id { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string? EventName { get; set; }
        public string? Description { get; set; }
        public EventStatus? EventStatus { get; set; }
        public EventType? EventType { get; set; }
        public string? Location { get; set; }
    }

    public class BasicEventResponseDTO : BasicEventRequestDTO {

        public EventType? EventType { get; set; }

        public List<EventImageDTO>? Images { get; set; }

        public List<EnrollmentDTO>? Enrollments { get; set; }
    }
}
