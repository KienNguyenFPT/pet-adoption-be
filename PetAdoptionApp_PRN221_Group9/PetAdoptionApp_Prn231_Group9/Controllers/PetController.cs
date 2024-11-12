using BusinessLogicLayer.IServices;
using BusinessLogicLayer.ViewModels.PetDTOs;
using BusinessObjects.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace PetAdoptionApp_Prn231_Group9.Controllers
{
    public class PetController : BaseController
    {
        private readonly IPetServices _petServices;

        public PetController(IPetServices petServices)
        {
            _petServices = petServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPets()
        {
            var result = await _petServices.GetAllPets();
            if (result == null)
            {
                return NotFound();
            } else
            {
                return Ok(result);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPet(Guid id)
        {
            var result = await _petServices.GetPetsById(id);
            if (result == null)
            {
                return NotFound();
            } else
            {
                return Ok(result);
            }
        }

        [Authorize(Roles = nameof(Role.Administrator) + "," + nameof(Role.Staff))]
        [HttpPost]
        public async Task<IActionResult> AddPet(PetDTOs petDTOs)
        {
            var result = await _petServices.CreatePets(petDTOs);
            if (result != null)
            {
                return StatusCode(201, result);
            } else
            {
                return BadRequest();
            }
        }

        [EnableCors("MyAllowSpecificOrigins")]
        [Authorize(Roles = nameof(Role.Administrator) + "," + nameof(Role.Staff))]
        [HttpPut("{petId}")]
        public async Task<IActionResult> UpdatePet(PetDTOs petDTOs, Guid petId)
        {
            var result = await _petServices.UpdatePets(petDTOs, petId);
            if (result != null)
            {
                return StatusCode(201, result);
            }
            else
            {
                return BadRequest();
            }
        }


        [EnableCors("MyAllowSpecificOrigins")]
        [Authorize(Roles = nameof(Role.Administrator) + "," + nameof(Role.Staff))]
        [HttpDelete("{petId}")]
        public async Task<IActionResult> DeletePet(Guid petId)
        {
            var result = await _petServices.DeletePets(petId);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
