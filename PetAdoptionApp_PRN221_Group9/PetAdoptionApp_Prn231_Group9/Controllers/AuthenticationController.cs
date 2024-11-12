using BusinessLogicLayer;
using BusinessLogicLayer.IServices;
using BusinessLogicLayer.ViewModels.UserDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace PetAdoptionApp_Prn231_Group9.Controllers
{
    public class AuthenticationController : BaseController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IClaimServices _claimServices;

        public AuthenticationController(IAuthenticationService authenticationService, IClaimServices claimServices)
        {
            _authenticationService = authenticationService;
            _claimServices = claimServices;
        }

        [EnableCors("MyAllowSpecificOrigins")]
        [HttpPost]
        public async Task<IActionResult> LoginWithEmailAndPasswordJWT(AuthenticationDTOs authenticationDTOs)
        {
            var result = await _authenticationService.LoginAccountService(authenticationDTOs);
            
            if(!result.Success)
            {
                return StatusCode(401, result);
            }else
            {
                return Ok(
                    new
                    {
                        success = result.Success,
                        message = result.Message,
                        token = result.Data
                    }

                    );
            }
        }

        [EnableCors("MyAllowSpecificOrigins")]
        [HttpPost]
        public async Task<IActionResult> RegisterAnAccount(RegistrationDTOs registrationDTOs)
        {
            var result = await _authenticationService.RegisterAccountService(registrationDTOs);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            else
            {
                return StatusCode(StatusCodes.Status201Created, result.Message);
            }
        }
        
        [EnableCors("MyAllowSpecificOrigins")]
        [HttpPost]
        public async Task<IActionResult> RegisterStaffAccount(RegistrationDTOs registrationDTOs)
        {
            var result = await _authenticationService.RegisterAsStaff(registrationDTOs);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            else
            {
                return StatusCode(StatusCodes.Status201Created, result.Message);
            }
        }

        [EnableCors("MyAllowSpecificOrigins")]
        [HttpPost]
        public async Task<IActionResult> RegisterAdminAccount(RegistrationDTOs registrationDTOs)
        {
            var result = await _authenticationService.RegisterAsAdministrator(registrationDTOs);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            else
            {
                return StatusCode(StatusCodes.Status201Created, result.Message);
            }
        }


        //For Azures 
        [EnableCors("MyAllowSpecificOrigins")]
        [HttpPost]
        public async Task<IActionResult> RegisterAccountUserOnAzureDeployment(RegistrationDTOs registrationDTOs)
        {
            var result = await _authenticationService.CreateUserAccountOnAzure(registrationDTOs);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            else
            {
                return StatusCode(StatusCodes.Status201Created, result.Message);
            }
        }
        [EnableCors("MyAllowSpecificOrigins")]
        [HttpPost]
        public async Task<IActionResult> RegisterAccountStaffOnAzureDeployment(RegistrationDTOs registrationDTOs)
        {
            var result = await _authenticationService.CreateStaffccountOnAzure(registrationDTOs);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            else
            {
                return StatusCode(StatusCodes.Status201Created, result.Message);
            }
        }

        [EnableCors("MyAllowSpecificOrigins")]
        [HttpPost]
        public async Task<IActionResult> RegisterAccountAdminOnAzureDeployment(RegistrationDTOs registrationDTOs)
        {
            var result = await _authenticationService.CreateAdminAccountOnAzure(registrationDTOs);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            else
            {
                return StatusCode(StatusCodes.Status201Created, result.Message);
            }
        }



        //Login first to check the user Role 
        //Then check the user if he/she's is correct or not 
        [Authorize]
        [HttpGet("verify/requiredRole")]
        public IActionResult VerifyUserAndRole(string requiredRole)
        {
            var currentUserId = _claimServices.GetCurrentUserId;

            // If there is no authenticated user
            if (currentUserId == Guid.Empty)
            {
                return Unauthorized("User not authenticated.");
            }

            // Check if the user is in the required role
            if (!User.IsInRole(requiredRole))
            {
                return Forbid("User does not have the required role.");
            }

            // Optionally verify against a specific user if needed
            var userIdFromClaim = User.FindFirst("Id")?.Value;

            // Return success response if the user and role match
            return Ok(new
            {
                Message = "User is verified and has the required role.",
                UserId = currentUserId,
                Role = requiredRole
            });
        
        }
    }
}
