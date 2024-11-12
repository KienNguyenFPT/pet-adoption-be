

namespace BusinessLogicLayer.ViewModels.ShelterDTOs
{
    public class ShelterDTO
    {
        public Guid? Id { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; }
        public int? LimitedCapacity { get; set; }
        public int? CurrentCapacity { get; set; }
    }
}
