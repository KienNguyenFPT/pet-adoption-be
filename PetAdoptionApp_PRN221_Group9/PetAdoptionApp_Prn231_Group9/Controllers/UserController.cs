using BusinessLogicLayer.IServices;
using BusinessLogicLayer.ViewModels.UserDTOs;
using BusinessObjects.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace PetAdoptionApp_Prn231_Group9.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserServices _userServices;

        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [Authorize(Roles = nameof(Role.Administrator))]
        [HttpGet("userList")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userServices.GetUsers();
            if (result == null)
            {
                return NotFound();
            }else
            {
                return Ok(result);
            }
        }
        [Authorize]
        [HttpGet("user/{Id}")]
        public async Task<IActionResult> GetUserById(Guid Id)
        {
            var result = await _userServices.GetUserById(Id);
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(result);
            }
        }
        [Authorize(Roles = nameof(Role.Administrator) + "," + nameof(Role.Staff) + "," + nameof(Role.User))]
        [HttpPut("updateUser/{UserId}")]
        public async Task<IActionResult> UpdateUserById([FromBody]UpdateUserDTO userDTO, Guid UserId)
        {
            var result = await _userServices.UpdateUserProfile(userDTO, UserId);
            if (result == null)
            {
                return  BadRequest();
            }else
            {
                return Ok(result);
            }
        }

        //On Going this is just a demo for delete an account, only update status for user Id
        [Authorize(Roles = nameof(Role.Administrator))]
        [HttpDelete("deleteUser/{id}")]
        public async Task<IActionResult> DeleteUserById(Guid id)
        {
            var result = await _userServices.DeleteUser(id);
            if (result != null)
            {
                return Ok(result);
            }else
            {
                return BadRequest(result);
            }
        }
    }
}
