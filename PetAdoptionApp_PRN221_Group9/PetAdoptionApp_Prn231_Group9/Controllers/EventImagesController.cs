using BusinessLogicLayer.IServices;
using BusinessLogicLayer.ViewModels.EventDTO;
using BusinessLogicLayer.ViewModels.PetImageDTOs;
using BusinessObjects.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PetAdoptionApp_Prn231_Group9.Controllers {
    public class EventImagesController : BaseController {

        private readonly IEventImagesService _service;

        public EventImagesController(IEventImagesService service) {
            _service = service;
        }

        [Authorize(Roles = nameof(Role.Administrator) + "," + nameof(Role.Staff))]
        [HttpPost("{eventId}")]
        public async Task<IActionResult> AddPhotoForEvent(Guid eventId, IFormFile file) {

            if (file == null || file.Length == 0) {
                return BadRequest("No file provided.");
            }
            var result = await _service.AddPhoto(file, eventId);
            if (result.Success) {
                return StatusCode(201, result);
            } else {
                return BadRequest(result.Message);
            }
        }


        [HttpGet("GetEventImages/{eventId}")]
        public async Task<IActionResult> GetEventImages(Guid eventId) {
            var result = await _service.GetEventImagesById(eventId);
            if (result != null) {
                return Ok(result);
            } else {
                return NotFound();
            }
        }


        
        [HttpDelete("Delete/{eventId}")]
        public async Task<IActionResult> DeleteEventPhoto(Guid photoId) {

            var result = await _service.DeletePhotos(photoId);
            if (result.Success) {
                return StatusCode(201, result);
            } else {
                return BadRequest(result.Message);
            }
        }

    }
}
