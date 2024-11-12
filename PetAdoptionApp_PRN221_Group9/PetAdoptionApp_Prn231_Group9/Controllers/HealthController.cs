using BusinessLogicLayer.IServices;
using BusinessLogicLayer.ViewModels.HealthDTO;
using BusinessObjects.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
namespace PetAdoptionApp_Prn231_Group9.Controllers
{
    
    public class HealthController : BaseController
    {
        private readonly IHealthServices _healthServices;

        public HealthController(IHealthServices healthServices)
        {
            _healthServices = healthServices;
        }

        /*[Authorize(Roles = nameof(Role.Administrator) + "," + nameof(Role.Staff) + "," + nameof(Role.User))]*/
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHealthById(Guid id)
        {
            var result = await _healthServices.GetHealthById(id);
            if (!result.Success)
            {
                return NotFound(result.Message);
            }
            return Ok(result.Data);
        }

        
        /*[Authorize(Roles = nameof(Role.Administrator) + "," + nameof(Role.Staff) + "," + nameof(Role.User))]*/
        [HttpGet]
        public async Task<IActionResult> GetAllHealths()
        {
            var result = await _healthServices.GetAllHealths();
            return Ok(result);
        }

        
        [Authorize(Roles = nameof(Role.Administrator) + "," + nameof(Role.Staff))]
        [HttpPost]
        public async Task<IActionResult> CreateHealth([FromBody] HealthDTOs healthDTOs)
        {
            var result = await _healthServices.CreateHealth(healthDTOs);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return StatusCode(201, result.Data);
        }

        
        [Authorize(Roles = nameof(Role.Administrator) + "," + nameof(Role.Staff))]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHealth([FromBody] HealthDTOs healthDTOs, Guid id)
        {
            var result = await _healthServices.UpdateHealth(healthDTOs, id);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);
        }

        
        [Authorize(Roles = nameof(Role.Administrator) + "," + nameof(Role.Staff))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHealth(Guid id)
        {
            var result = await _healthServices.DeleteHealth(id);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok();
        }
    }
}