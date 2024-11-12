using AutoMapper;
using Azure;
using BusinessLogicLayer.IServices;
using BusinessLogicLayer.ViewModels.AdoptionsDTOs;
using BusinessObjects;
using DataAccessObjects.ServicesResponses;
using Org.BouncyCastle.Asn1.Cms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class AdoptionServices : IAdoptionServices
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClaimServices _claimServices;
        private readonly ICurrentTimeServices _currentTimeServices;

        public AdoptionServices(IMapper mapper, IUnitOfWork unitOfWork, IClaimServices claimServices, ICurrentTimeServices currentTimeServices)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _claimServices = claimServices;
            _currentTimeServices = currentTimeServices;

        }




        public async Task<ServicesResponses<CreateAdoptionDTOs>> CreateAdoptions(CreateAdoptionDTOs adoptionDTOs, Guid PetId)
        {
            var response = new ServicesResponses<CreateAdoptionDTOs>();
            try
            {
                var getUser = _claimServices.GetCurrentUserId;
                if (getUser == null)
                {
                    response.Success = false;
                    response.Message = "You need to login first to use this function";
                }
                var getPet = await _unitOfWork._petRepo.GetByIdAsync(PetId);
                if (getPet == null)
                {
                    response.Success = false;
                    response.Message = "Failed to find pet";
                }


                var mapper = _mapper.Map<Adoption>(adoptionDTOs);
                mapper.UserId = _claimServices.GetCurrentUserId;
                mapper.PetId = getPet.Id;
                mapper.AdoptionStatus = BusinessObjects.Enum.AdoptionStatus.Pending;
                mapper.ApprovalDate = _currentTimeServices.GetCurrentTime().AddDays(5);
                mapper.ApplicationDate = _currentTimeServices.GetCurrentTime();
                await _unitOfWork._adoptionRepo.AddAsync(mapper);
                var IsSuccess = await _unitOfWork.SaveChangeAsync() > 0;

                if (IsSuccess)
                {
                    response.Success = true;
                    response.Message = "Add Adoption Successfully";
                    response.Data = _mapper.Map<CreateAdoptionDTOs>(mapper);
                }else
                {
                    response.Success =false;
                    response.Message = "Failed to add an adoption";
                }

            }catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };

            }
            return response;
        }

        public async Task<ServicesResponses<bool>> DeleteAdoption(Guid id)
        {
            var response = new ServicesResponses<bool>();

            try
            {
                var getUser = _claimServices.GetCurrentUserId;

                if (getUser == null)
                {
                    response.Success = false;
                    response.Message = "You need to login first to use this function";
                }
                var getId = await _unitOfWork._adoptionRepo.GetByIdAsync(id);
                if (getId == null)
                {
                    response.Success = false;
                    response.Message = "Could not find the adoption file";
                }
                 _unitOfWork._adoptionRepo.Delete(getId);
                var IsSucces = await _unitOfWork.SaveChangeAsync() > 0;
                if (IsSucces)
                {
                    response.Success = true;
                    response.Message = "Delete successfully";
                }else
                {
                    response.Success=false;
                    response.Message = "Failed to delete";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };

            }
            return response;
        }

        public async Task<ServicesResponses<GetAdoptionDTos>> GetAdoption(Guid id)
        {
            var response = new ServicesResponses<GetAdoptionDTos>();
            try
            {
                var getUser = _claimServices.GetCurrentUserId;

                if (getUser == null)
                {
                    response.Success = false;
                    response.Message = "You need to login first to use this function";
                }
                var result = await _unitOfWork._adoptionRepo.GetByIdAsync(id);
                if (result != null)
                {
                    response.Success = true;
                    response.Message = "Retrieved Data Successfully";
                    response.Data = _mapper.Map<GetAdoptionDTos>(result);
                }else
                {
                    response.Success = false;
                    response.Message = "Failed to retrieve data";
                }


            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;
        }

        public async Task<ServicesResponses<GetAdoptionDTos>> GetAdoptionByPetId(Guid PetId)
        {
            var response = new ServicesResponses<GetAdoptionDTos>();
            try
            {
                var result = await _unitOfWork._adoptionRepo.GetAdoptionByPetId(PetId);
                if (result != null)
                {
                    response.Success= true;
                    response.Message = "Retrieve Data Successfully";
                    response.Data = _mapper.Map<GetAdoptionDTos>(result);
                }else
                {
                    response.Success = false;
                    response.Message = "Failed to retrieve data";
                    return response;
                }
            }catch (Exception ex) 
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;
        }

        public async Task<ServicesResponses<IEnumerable<GetAdoptionDTos>>> GetAdoptionByUserId(Guid UserId)
        {
            var response = new ServicesResponses<IEnumerable<GetAdoptionDTos>>();
            try
            {
                var result = await _unitOfWork._adoptionRepo.GetAdoptionListByUser(UserId);
                if (result != null)
                {
                    response.Success = true;
                    response.Message = "Retrieve Data Successfully";
                    response.Data = _mapper.Map<IEnumerable<GetAdoptionDTos>>(result);
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to retrieve data";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;
        }

        public async Task<ServicesResponses<IEnumerable<GetAdoptionDTos>>> GetAllAdoptions()
        {
            var response = new ServicesResponses<IEnumerable<GetAdoptionDTos>>();
            try
            {
                var getUserId = _claimServices.GetCurrentUserId;
                if (getUserId == null)
                {
                    response.Success = false;
                    response.Message = "You need to login first to use this function";
                }
                var result = await _unitOfWork._adoptionRepo.GetAllAsync();
                if (result != null)
                {
                    response.Success = true;
                    response.Message = "Retrieved Data Successfully";
                    response.Data = _mapper.Map<IEnumerable<GetAdoptionDTos>>(result);
                }else
                {
                    response.Success = false;
                    response.Message = "Failed to retrieve data";
                }



            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;
        }

        public async Task<ServicesResponses<AdoptionDTOs>> UpdateAdoptions(AdoptionDTOs adoptionDTOs, Guid Id)
        {
            var response = new ServicesResponses<AdoptionDTOs>();

            try
            {
                
                var userId = _claimServices.GetCurrentUserId;
                if (userId == null)
                {
                    response.Success = false;
                    response.Message = "You need to login first to use this function";
                }
                var GetId = await _unitOfWork._adoptionRepo.GetByIdAsync(Id);
                if (GetId == null)
                {
                    response.Success = false;
                    response.Message = "Could not find any adoption form";
                }
                _mapper.Map(adoptionDTOs, GetId);
                _unitOfWork._adoptionRepo.Update(GetId);
                var IsSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (IsSuccess)
                {
                    response.Success = true;
                    response.Message = "Update Successfully";
                    response.Data = _mapper.Map<AdoptionDTOs>(GetId);
                }else
                {
                    response.Success = false;
                    response.Message = "Failed to update";
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
