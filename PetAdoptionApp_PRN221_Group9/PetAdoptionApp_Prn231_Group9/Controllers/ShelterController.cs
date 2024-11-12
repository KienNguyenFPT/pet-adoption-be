using BusinessLogicLayer.IServices;
using BusinessLogicLayer.ViewModels.ShelterDTOs;
using BusinessObjects.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PetAdoptionApp_Prn231_Group9.Controllers {
    public class ShelterController : BaseController {
        private readonly IShelterServices _shelterServices;

        public ShelterController(IShelterServices shelterServices) {
            _shelterServices = shelterServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllShelters() {
            var result = await _shelterServices.ListAllShelters();
            if (result.Success) {
                return Ok(result);
            }
            else {
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
        }
        
        [HttpPost]
        [Authorize(Roles = nameof(Role.Administrator) + "," + nameof(Role.Staff))]
        public async Task<IActionResult> CreateShelter([FromBody] ShelterDTO shelterDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdShelter = await _shelterServices.CreateShelterAsync(shelterDTO);
            
            return Created("Shelter", createdShelter);
        }
        [Authorize(Roles = nameof(Role.Administrator) + "," + nameof(Role.Staff))]
        [HttpPut]
        public async Task<IActionResult> UpdateShelter([FromBody] ShelterDTO shelterDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _shelterServices.UpdateShelterAsync(shelterDTO);
            if (result == null) {
                return NotFound("Shelter not found");
            }
            return Ok(result);
        }
        [Authorize(Roles = nameof(Role.Administrator) + "," + nameof(Role.Staff))]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteShelter(Guid id)
        {
            var result = await _shelterServices.DeleteShelterAsync(id);
            if (!result) {
                return NotFound("Shelter not found");
            }

            return Ok("Shelter deleted");
        }

    }
}
