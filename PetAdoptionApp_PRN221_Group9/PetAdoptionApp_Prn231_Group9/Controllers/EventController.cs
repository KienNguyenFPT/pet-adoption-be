using System.Security.Claims;
using BusinessLogicLayer.IServices;
using BusinessLogicLayer.ViewModels.EventDTO;
using BusinessObjects.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PetAdoptionApp_Prn231_Group9.Controllers {
    public class EventController : BaseController {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService) {
            this._eventService = eventService;
        }

        [Authorize(Roles = nameof(Role.Staff) + "," + nameof(Role.Administrator))]
        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] BasicEventRequestDTO eventDTO) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _eventService.CreateEventAsync(eventDTO);
            if (!result.Success) {
                return StatusCode(500, "Error creating the event");
            }

            return CreatedAtAction(nameof(CreateEvent), new { id = result.Data.Id }, result.Data); // 201 Created
        }


        [Authorize(Roles = nameof(Role.Staff) + "," + nameof(Role.Administrator))]
        [HttpPut]
        public async Task<IActionResult> UpdateEvent([FromBody] BasicEventRequestDTO eventDTO) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _eventService.UpdateEventAsync(eventDTO);
            if (!result.Success) {
                return StatusCode(500, result.Message);
            }

            return Ok(result); // 200 OK
        }



        [Authorize(Roles = nameof(Role.Staff) + "," + nameof(Role.Administrator))]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteEvent(Guid id) {
            var result = await _eventService.DeleteEventAsync(id);
            if (!result.Success)
                return StatusCode(500, result.Message);

            return Ok("Delete successfully"); // 204 No Content
        }


        [HttpGet]
        public async Task<IActionResult> GetAllEvents() {
            var result = await _eventService.GetAllEventsAsync();
            if (!result.Success)
                return StatusCode(500, result.Message);

            return Ok(result); // 200 OK with list of events
        }


        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetEventById(Guid id) {
            var result = await _eventService.GetEventByIdAsync(id);
            if (!result.Success)
                return StatusCode(500, result.Message);

            if (result.Data == null)
                return NotFound("Event not found"); // 404 Not Found

            return Ok(result); // 200 OK with event data
        }


        [Authorize(Roles = nameof(Role.User) + "," + nameof(Role.Administrator))]
        [HttpPost("enroll/{eventId:guid}")]
        public async Task<IActionResult> UserEnrollEvent(Guid eventId) {
            var result = await _eventService.UserEnrollEvent(eventId);
            if (!result.Success)
                return StatusCode(500, result.Message);

            return Ok(result);
        }

        [Authorize(Roles = nameof(Role.User))]
        [HttpGet("userEnrollments")]
        public async Task<IActionResult> GetAllUserEnrollment() {
            var result = await _eventService.GetEnrollmentOfUser();
            return Ok(result);
        }

        [Authorize(Roles = nameof(Role.User))]
        [HttpGet("Get/{userId}")]
        public async Task<IActionResult> GetUserThatHasBeenEnrolled(Guid userId)
        {
            var result = await _eventService.GetUserEnrolledEvent(userId);
            if (result != null)
            {
                return  Ok(result);
            }else
            {
                return NotFound();
            }
        }

    }
}
