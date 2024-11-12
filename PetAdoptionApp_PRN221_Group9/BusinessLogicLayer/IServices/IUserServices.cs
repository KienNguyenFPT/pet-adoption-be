using BusinessLogicLayer.ViewModels.UserDTOs;
using DataAccessObjects.ServicesResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.IServices
{
    public interface IUserServices
    {
        public Task<ServicesResponses<IEnumerable<UserDTO>>> GetUsers();
        public Task<ServicesResponses<UserDTO>> GetUserById(Guid Id);
        public Task<ServicesResponses<UserDTO>> UpdateUserProfile(UpdateUserDTO UserDTO, Guid Id);
        public Task<ServicesResponses<bool>> DeleteUser(Guid Id);
    }
}
