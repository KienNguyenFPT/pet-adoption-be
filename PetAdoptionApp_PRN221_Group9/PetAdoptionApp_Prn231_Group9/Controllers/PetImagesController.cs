using BusinessLogicLayer.IServices;
using BusinessLogicLayer.ViewModels.PetImageDTOs;
using BusinessObjects.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace PetAdoptionApp_Prn231_Group9.Controllers
{
    public class PetImagesController : BaseController
    {
        private readonly IPetImageServices _services;

        public PetImagesController(IPetImageServices petImageServices)
        {
            _services = petImageServices;
        }

        [Authorize(Roles = nameof(Role.Administrator) + "," + nameof(Role.Staff))]
        [HttpPost("AddPhoto/{petId}")]
        public async Task<IActionResult> AddPetPhotos(Guid petId, IFormFile file)
        {
            
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file provided.");
            }
            var petImagesDTOs = new PetImagesDTOs();
            var result = await _services.AddPhotos(file, petImagesDTOs, petId);
            if (result.Success)
            {
                return StatusCode(201, result);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }
        [HttpGet("/GetAllPhotos")]
        public async Task<IActionResult> GetAllPhoto()
        {
            var result = await _services.GetPetImagesLists();
            if (result == null)
            {
                return NotFound();
            }else
            {
                return Ok(result);
            }
        }
        [HttpGet("/GetPhotos/{Id}")]
        public async Task<IActionResult> GetPhotoById(Guid Id)
        {
            var result = await _services.GetPetImageById(Id);
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(result);
            }
        }


        [Authorize(Roles = nameof(Role.Administrator) + "," + nameof(Role.Staff))]
        [HttpDelete("Delete/{photoId}")]
        public async Task<IActionResult> DeletePetPhoto(Guid photoId)
        {
            
            var result = await _services.DeletePhotos(photoId);
            if (result.Success)
            {
                return StatusCode(201, result);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }
        [HttpGet("GetPetImages/{petId}")]
        public async Task<IActionResult> GetPetListImage(Guid petId)
        {
            var result = await _services.GetPetImagesById(petId);
            if (result != null) 
            {
                return Ok(result);
            }else
            {
                return NotFound();
            }
        }
        

    }
}
