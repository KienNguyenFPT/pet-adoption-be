using AutoMapper;
using Azure;
using BusinessLogicLayer.IServices;
using BusinessLogicLayer.ViewModels.UserDTOs;
using BusinessObjects;
using DataAccessObjects.ServicesResponses;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class UserServices : IUserServices
    {
        private readonly IClaimServices _claimServices;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserServices(IClaimServices claimServices, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _claimServices = claimServices;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }





        public async Task<ServicesResponses<bool>> DeleteUser(Guid Id)
        {
            var response = new ServicesResponses<bool>();
            try
            {
                var getId = _claimServices.GetCurrentUserId;
                if(getId == null)
                {
                    response.Success = false;
                    response.Message = "Please log in first to access this function";
                    return response;
                }
                var userId = await _unitOfWork._userRepo.GetByIdAsync(Id);
                if(userId == null)
                {
                    response.Success = false;
                    response.Message = "Could not find user";
                    return response;
                }
                else
                {
                    //Only Change Status 

                    _unitOfWork._userRepo.SoftRemove(userId);
                    var IsSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                    if(IsSuccess)
                    {
                        response.Success = true;
                        response.Message = "Delete Successfully";
                    }else
                    {
                        response.Success = false;
                        response.Message = "Fail to delete user";
                    }
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;

        }

        public async Task<ServicesResponses<UserDTO>> GetUserById(Guid Id)
        {
            var response = new ServicesResponses<UserDTO>();
            try
            {
                var result = await _unitOfWork._userRepo.GetByIdAsync(Id);

                if (result == null)
                {
                    response.Success = false;
                    response.Message = "Fail to find user";
                    return response;
                }else
                {
                    response.Success= true;
                    response.Message = "Retrieve data successfully";
                    response.Data = _mapper.Map<UserDTO>(result);
                    return response;
                }
            }catch(Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;
        }

        public async Task<ServicesResponses<IEnumerable<UserDTO>>> GetUsers()
        {
            var response = new ServicesResponses<IEnumerable<UserDTO>>();
            try
            {
                var result = await _unitOfWork._userRepo.GetAllAsync();
                if (result == null)
                {
                    response.Success = false;
                    response.Message = "Fail to retrieve data";
                    return response;
                }else
                {
                    response.Success = true;
                    response.Message = "Retrieved Data Successfully";
                    response.Data = _mapper.Map<IEnumerable<UserDTO>>(result);
                    return response;
                }




            }catch(Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;




        }

        public async Task<ServicesResponses<UserDTO>> UpdateUserProfile(UpdateUserDTO UserDTO, Guid Id)
        {
            var response = new ServicesResponses<UserDTO>();
            try
            {
                var getUserId =  _claimServices.GetCurrentUserId;
                if (getUserId == null)
                {
                    response.Success = false;
                    response.Message = "You need to login first to use this function";
                }
                var getUser = await _unitOfWork._userRepo.GetByIdAsync(Id);
                if (getUser == null)
                {
                    response.Success = false;
                    response.Message = "Fail to find the user";
                }
                var mapper = _mapper.Map(UserDTO, getUser);

                _unitOfWork._userRepo.Update(mapper);
                var IsSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (IsSuccess)
                {
                    response.Success = true;
                    response.Message = "Update User Successfully";
                    response.Data = _mapper.Map<UserDTO>(mapper);
                }else
                {
                    response.Success = false;
                    response.Message = "Fail to update data";
                }




            }catch(Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;

        }
    }
}
