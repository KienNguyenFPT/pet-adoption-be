using BusinessLogicLayer.ViewModels.EventDTO;
using DataAccessObjects.ServicesResponses;

namespace BusinessLogicLayer.IServices {
    public interface IEventService {

        Task<ServicesResponses<EventDTO>> CreateEventAsync(BasicEventRequestDTO eventDto);
        Task<ServicesResponses<EventDTO>> UpdateEventAsync(BasicEventRequestDTO eventDto);
        Task<ServicesResponses<bool>> DeleteEventAsync(Guid id);
        Task<ServicesResponses<IEnumerable<BasicEventResponseDTO>>> GetAllEventsAsync();
        Task<ServicesResponses<BasicEventResponseDTO>> GetEventByIdAsync(Guid id);
        Task<ServicesResponses<EnrollmentDTO>> UserEnrollEvent(Guid eventId);
        Task<ServicesResponses<IEnumerable<EventDTO>>> GetEnrollmentOfUser();
        Task<ServicesResponses<IEnumerable<EventHistoryDTO>>> GetUserEnrolledEvent(Guid userId);
    }
}
