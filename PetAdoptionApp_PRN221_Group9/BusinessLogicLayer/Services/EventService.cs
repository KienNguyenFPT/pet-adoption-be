using AutoMapper;
using BusinessLogicLayer.IServices;
using BusinessLogicLayer.ViewModels.EventDTO;
using BusinessObjects;
using BusinessObjects.Enum;
using DataAccessObjects.ServicesResponses;
using Microsoft.AspNetCore.Authorization;

namespace BusinessLogicLayer.Services {
    public class EventService : IEventService {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IClaimServices _claimServices;
        private readonly ICurrentTimeServices _currentTimeServices;


        public EventService(IUnitOfWork unitOfWork, IMapper mapper, IClaimServices claimServices, ICurrentTimeServices currentTimeServices = null) {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _claimServices = claimServices;
            _currentTimeServices = currentTimeServices;
        }


        public async Task<ServicesResponses<EventDTO>> CreateEventAsync(BasicEventRequestDTO eventDTO) {
            var response = new ServicesResponses<EventDTO>();

            try {
                var eventEntity = _mapper.Map<Event>(eventDTO);
                eventEntity.Id = Guid.NewGuid(); // Generate new Guid for the event
                eventEntity.StartDate = _currentTimeServices.GetCurrentTime();
                eventEntity.EndDate = _currentTimeServices.GetCurrentTime().AddDays(5);

                await _unitOfWork._eventRepo.AddAsync(eventEntity);
                await _unitOfWork.SaveChangeAsync();

                // Map back to DTO to return
                var createdEventDTO = _mapper.Map<EventDTO>(eventEntity);

                response.Data = createdEventDTO;
                response.Success = true;
                response.Message = "Event created successfully";
                return response;
            } catch (Exception ex) {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }
        }

        public async Task<ServicesResponses<EventDTO>> UpdateEventAsync(BasicEventRequestDTO eventDTO) {
            var response = new ServicesResponses<EventDTO>();
            if (eventDTO.Id == null) {
                response.Success = false;
                response.Message = "Id is required";
                return response;
            }

            try {
                // Find existing event by id
                var existingEvent = await _unitOfWork._eventRepo.GetByIdAsync(eventDTO.Id.Value);
                if (existingEvent == null) {
                    response.Success = false;
                    response.Message = "Event not found";
                    return response;
                }

                // Update the existing event with the new data
                MapBasicRequestToEntity(eventDTO, existingEvent);

                _unitOfWork._eventRepo.Update(existingEvent);
                await _unitOfWork.SaveChangeAsync();

                // Map back to DTO to return
                var updatedEventDto = _mapper.Map<EventDTO>(existingEvent);

                response.Data = updatedEventDto;
                response.Success = true;
                response.Message = "Event updated successfully";
                return response;
            } catch (Exception ex) {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }
        }
        public async Task<ServicesResponses<bool>> DeleteEventAsync(Guid id) {
            var response = new ServicesResponses<bool>();

            try {
                // Find the existing event by id
                var existingEvent = await _unitOfWork._eventRepo.GetByIdAsync(id);
                if (existingEvent == null) {
                    response.Success = false;
                    response.Message = "Event not found";
                    return response;
                }

                _unitOfWork._eventRepo.Delete(existingEvent);
                await _unitOfWork.SaveChangeAsync();

                response.Data = true;
                response.Success = true;
                response.Message = "Event deleted successfully";
                return response;
            } catch (Exception ex) {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }
        }
        public async Task<ServicesResponses<IEnumerable<BasicEventResponseDTO>>> GetAllEventsAsync() {
            var response = new ServicesResponses<IEnumerable<BasicEventResponseDTO>>();

            try {
                // Get all events from the repository
                var events = await _unitOfWork._eventRepo.GetEventsWithRelationship();

                // Map the event entities to DTOs
                var eventDTOs = _mapper.Map<IEnumerable<BasicEventResponseDTO>>(events);

                response.Data = eventDTOs;
                response.Success = true;
                response.Message = "Events retrieved successfully";
                return response;
            } catch (Exception ex) {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }
        }

        public async Task<ServicesResponses<BasicEventResponseDTO>> GetEventByIdAsync(Guid id) {
            var response = new ServicesResponses<BasicEventResponseDTO>();

            try {
                // Retrieve event by id from the repository
                var eventEntity = await _unitOfWork._eventRepo.GetEventWithRelationshipById(id);
                if (eventEntity == null) {
                    response.Success = false;
                    response.Message = "Event not found";
                    return response;
                }

                // Map the event entity to a DTO
                var eventDTO = _mapper.Map<BasicEventResponseDTO>(eventEntity);

                response.Data = eventDTO;
                response.Success = true;
                response.Message = "Event retrieved successfully";
                return response;
            } catch (Exception ex) {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }
        }



        private void MapBasicRequestToEntity(BasicEventRequestDTO eventDTO, Event existingEvent) {
            existingEvent.EventName = eventDTO.EventName;
            existingEvent.Description = eventDTO.Description;
            existingEvent.StartDate = eventDTO.StartDate;
            existingEvent.EndDate = eventDTO.EndDate;
            existingEvent.Location = eventDTO.Location;
            existingEvent.EventStatus = eventDTO.EventStatus;
            existingEvent.EventType = eventDTO.EventType;
        }

        public async Task<ServicesResponses<EnrollmentDTO>> UserEnrollEvent(Guid eventId) {
            var eventValidationResponse = await ValidateEventAsync(eventId);
            if (!eventValidationResponse.Success) {
                return eventValidationResponse;
            }

            var userId = await GetCurrentUserAsync();
            if (userId == null) {
                return new ServicesResponses<EnrollmentDTO> {
                    Success = false,
                    Message = "User is not authenticated.",
                   
                };
            }

            var isAlreadyEnrolled = await IsUserAlreadyEnrolledAsync(eventId, userId.Value);
            if (isAlreadyEnrolled) {
                return new ServicesResponses<EnrollmentDTO> {
                    Success = false,
                    Message = "User is already enrolled in this event.",

                };
            }

            return await EnrollUserAsync(eventId, userId.Value);
        }

        private async Task<ServicesResponses<EnrollmentDTO>> ValidateEventAsync(Guid eventId) {
            var prjEvent = await _unitOfWork._eventRepo.GetByIdAsync(eventId);

            if (prjEvent == null) {
                return new ServicesResponses<EnrollmentDTO> {
                    Success = false,
                    Message = "Event not found.",
                    
                };
            }

            if (prjEvent.EventStatus != EventStatus.Open) {
                return new ServicesResponses<EnrollmentDTO> {
                    Success = false,
                    Message = "This event is not open for enrollment.",
                    
                };
            }

            return new ServicesResponses<EnrollmentDTO> { Success = true };
        }

        private async Task<Guid?> GetCurrentUserAsync() {
            var userId = _claimServices.GetCurrentUserId;
            return userId;
        }

        private async Task<bool> IsUserAlreadyEnrolledAsync(Guid eventId, Guid userId) {
            var existingEnrollment = await _unitOfWork._eventEnrollmentRepo.GetEnrollmentAsync(eventId, userId);

            return existingEnrollment != null;
        }

        private async Task<ServicesResponses<EnrollmentDTO>> EnrollUserAsync(Guid eventId, Guid userId) {
            var response = new ServicesResponses<EnrollmentDTO>();
            try
            {
                var newEnrollment = new Enrollment
                {
                    Id =  Guid.NewGuid(),
                    EventId = eventId,
                    UserId = userId,
                    // Optionally set other fields like created date here
                };

                await _unitOfWork._eventEnrollmentRepo.AddAsync(newEnrollment);
                var IsSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (IsSuccess)
                {
                    response.Data = _mapper.Map<EnrollmentDTO>(newEnrollment);
                    response.Success = true;
                    response.Message = "User Enrolled Successfully";
                }else
                {
                    response.Success = false;
                    response.Message = "User Failed to enroll";
                }

            }
            catch (Exception ex)
            {
                response.Success=false;
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;




           
        }

        public async Task<ServicesResponses<IEnumerable<EventDTO>>> GetEnrollmentOfUser() {
            var userId = _claimServices.GetCurrentUserId;
            var events = await _unitOfWork._eventRepo.GetEventOfUser(userId);

            var dto = _mapper.Map<IEnumerable<EventDTO>>(events);

            return new ServicesResponses<IEnumerable<EventDTO>>() {
                Success = true,
                Data = dto
            };
        }

        public async Task<ServicesResponses<IEnumerable<EventHistoryDTO>>> GetUserEnrolledEvent(Guid userId)
        {
            var response = new ServicesResponses<IEnumerable<EventHistoryDTO>>();
            try
            {
                userId = _claimServices.GetCurrentUserId;
                
                if (userId == Guid.Empty)
                {
                    response.Success = false;
                    response.Message = "You should login first to use this function";
                }
                
                var result = await _unitOfWork._eventEnrollmentRepo.GetUserEnrollmentsAsync(userId);
                if (result != null)
                {
                    response.Success = true;
                    response.Message = "Retrieve Data Successfully";
                    response.Data = _mapper.Map<IEnumerable<EventHistoryDTO>>(result);
                }else
                {
                    response.Success =false;
                    response.Message = "Fail to retrieve data";
                }

            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return response;
        }
    }
}
