using BusinessObjects.Enum;

namespace BusinessLogicLayer.ViewModels.UserDTOs {
    public class UserDTO {
        public Guid? Id { get; set; }
        public string? EmailAddress { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public Role Role { get; set; }
        /*public bool IsConfirmed { get; set; }*/
    }
}
