using AutoMapper;
using BusinessLogicLayer.Commons;
using BusinessLogicLayer.IServices;
using BusinessLogicLayer.ViewModels.PetDTOs;
using BusinessObjects;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DataAccessObjects.ServicesResponses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class PetServices : IPetServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IClaimServices _claimServices;
        private readonly ICurrentTimeServices _currentTimeServices;
        


        public PetServices(IClaimServices claimServices, ICurrentTimeServices currentTimeServices, IMapper mapper, IUnitOfWork unitOfWork
            )
        {
            _claimServices = claimServices;
            _currentTimeServices = currentTimeServices;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            
        }

        

        public async Task<ServicesResponses<PetDTOs>> CreatePets(PetDTOs petDTOs)
        {
            var response = new ServicesResponses<PetDTOs>();
            try
            {
                var mapping = _mapper.Map<Pet>(petDTOs);
                var getId = _claimServices.GetCurrentUserId;
                if (getId == null)
                {
                    response.Success = false;
                    response.Message = "You need to login first";
                    
                }else
                {
                    mapping.RescuedDate = _currentTimeServices.GetCurrentTime();
                    await _unitOfWork._petRepo.AddAsync(mapping);
                    var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                    if (isSuccess) 
                    {
                        var mappingResult = _mapper.Map<PetDTOs>(mapping);
                        response.Success = true;
                        response.Message = "Add Successfully";
                        response.Data = mappingResult;
                    }else
                    {
                        response.Success = false;
                        response.Message = "Failed to delete data";
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

        public async Task<ServicesResponses<bool>> DeletePets(Guid petId)
        {
            var response = new ServicesResponses<bool>();
            try
            {
                var getUserId = _claimServices.GetCurrentUserId;
                if (getUserId == null)
                {
                    response.Success = false;
                    response.Message = "Please login first to use this function";
                }
                

                var getPetId = await _unitOfWork._petRepo.GetByIdAsync(petId);
                if (getPetId != null)
                {
                    _unitOfWork._petRepo.Delete(getPetId);
                    var IsSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                    if (IsSuccess)
                    {
                        response.Success = true;
                        response.Message = "Delete Successfully";
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Unable to delete";
                    }
                }
                else
                {
                    response.Success = false;
                    response.Message = "Unable to find pet";
                }







            }catch  (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }




            return response;
        }

        public async Task<ServicesResponses<IEnumerable<GetPetDTOs>>> GetAllPets()
        {
            var response = new ServicesResponses<IEnumerable<GetPetDTOs>>();   

            try
            {
                var result = await _unitOfWork._petRepo.GetAllPetAndShelter();
                if (result != null)
                {
                    response.Success = true;
                    response.Message = "Data retrieved successfully";
                    response.Data = _mapper.Map<IEnumerable<GetPetDTOs>>(result);

                }else
                {
                    response.Success = false;
                    response.Message = "Failed to retrieve data";
                }

            }catch  (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;
        }

        public async Task<ServicesResponses<GetPetDTOs>> GetPetsById(Guid id)
        {
            var response = new ServicesResponses<GetPetDTOs>();
            try
            {
                var result = await _unitOfWork._petRepo.GetPetById(id);
                if (result != null)
                {
                    response.Success = true;
                    response.Message = "Retrieved Data Successfully";
                    response.Data = _mapper.Map<GetPetDTOs>(result);
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to retrieve Data";
                    
                }




            }catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;
        }

        public async Task<ServicesResponses<PetDTOs>> UpdatePets(PetDTOs petDTOs, Guid petId)
        {
            var response = new ServicesResponses<PetDTOs>();
            try
            {
                var getUserId = _claimServices.GetCurrentUserId;
                if (getUserId == null)
                {
                    response.Success = false;
                    response.Message = "You need to login first to use this function";
                }
               
                var getPetId = await _unitOfWork._petRepo.GetByIdAsync(petId);
                if (getPetId != null)
                {
                    var mapping = _mapper.Map(petDTOs, getPetId);
                    _unitOfWork._petRepo.Update(mapping);
                    var IsSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                    if (IsSuccess)
                    {
                        response.Success = true;
                        response.Message = "Update Successfully";
                        response.Data = _mapper.Map<PetDTOs>(mapping);
                    }else
                    {
                        response.Success = false;
                        response.Message = "Failed To Update";
                    }
                } else
                {
                    response.Success = false;
                    response.Message = "Failed To find pet";
                }




            }catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;

        }
    }
}
