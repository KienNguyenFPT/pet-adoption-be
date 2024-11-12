using BusinessLogicLayer.IServices;
using BusinessLogicLayer.ViewModels.AdoptionsDTOs;
using BusinessObjects.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PetAdoptionApp_Prn231_Group9.Controllers
{
    public class AdoptionController : BaseController
    {
        private readonly IAdoptionServices _adoptionServies;

        public AdoptionController(IAdoptionServices adoptionServices)
        {
            _adoptionServies = adoptionServices;
        }

        [Authorize(Roles = nameof(Role.Staff) + "," + nameof(Role.User) + "," + nameof(Role.Administrator))]
        [HttpGet("AdoptionForm")]
        public async Task<IActionResult> GetAllAdoptionForms()
        {
            var result = await _adoptionServies.GetAllAdoptions();
            if (result == null)
            {
                return NotFound();
            }else
            {
                return Ok(result);
            }
        }
        [Authorize(Roles = nameof(Role.Staff) + "," + nameof(Role.User) + "," + nameof(Role.Administrator))]
        [HttpGet("AdoptionForm/{Id}")]
        public async Task<IActionResult> GetAdoption( Guid Id)
        {
            var result = await _adoptionServies.GetAdoption(Id);
            if (result == null) 
            {
                return NotFound();
            }else
            {
                return Ok(result);
            }
        }
        //Only User can adopt a pet
        [Authorize(Roles = nameof(Role.User) + "," + nameof(Role.Administrator))]
        [HttpPost("AddAdoptionForm/{PetId}")]
        public async Task<IActionResult> AddAdoptionForm([FromBody] CreateAdoptionDTOs adoptionDTOs, Guid PetId)
        {
            var result = await _adoptionServies.CreateAdoptions(adoptionDTOs, PetId);
            if (result != null)
            {
                return StatusCode(201, result);
            }else
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = nameof(Role.Staff) + "," + nameof(Role.Administrator))]
        [HttpPut("UpdateAdoptionForm/{Id}")]
        public async Task<IActionResult> UpdateAdoption([FromBody] AdoptionDTOs adoptionDTOs, Guid Id)
        {
            var result = await _adoptionServies.UpdateAdoptions(adoptionDTOs, Id);
            if (result != null)
            {
                return Ok(result);
            } else
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = nameof(Role.Staff) + "," + nameof(Role.Administrator))]
        [HttpDelete("DeleteAdoptionForm/{Id}")]
        public async Task<IActionResult> UpdateAdoption(Guid Id)
        {
            var result = await _adoptionServies.DeleteAdoption(Id);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }
        [Authorize(Roles = nameof(Role.Staff) + "," + nameof(Role.Administrator))]
        [HttpGet("AdoptionPet/{PetId}")]
        public async Task<IActionResult> GetAdoptionByPetId(Guid PetId)
        {
            var result = await _adoptionServies.GetAdoptionByPetId(PetId);
            if(result != null)
            {
                return Ok(result);
            }else
            {
                return NotFound();
            }
        }
        [Authorize(Roles = nameof(Role.Staff) + "," + nameof(Role.Administrator) + "," + nameof(Role.User))]
        [HttpGet("getAdoptionsByUserId/{userId}")]
        public async Task<IActionResult> GetAdoptionPetbyUserId(Guid userId)
        {
            var result = await _adoptionServies.GetAdoptionByUserId(userId);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
