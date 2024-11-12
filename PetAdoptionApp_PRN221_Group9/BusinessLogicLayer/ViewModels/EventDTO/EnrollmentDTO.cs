using BusinessLogicLayer.ViewModels.UserDTOs;

namespace BusinessLogicLayer.ViewModels.EventDTO {
    public class EnrollmentDTO {
        public Guid? Id { get; set; }
        public Guid? UserId { get; set; }
        public Guid? EventId { get; set; }
        public UserDTO? User { get; set; }  
    }
}
