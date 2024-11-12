using AutoMapper;
using BusinessLogicLayer.IServices;
using DataAccessObjects.ServicesResponses;
using BusinessLogicLayer.ViewModels.ShelterDTOs;
using BusinessObjects;

namespace BusinessLogicLayer.Services {
    public class ShelterServices : IShelterServices {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;


        public ShelterServices(IMapper mapper, IUnitOfWork unitOfWork) {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }


        public async Task<ServicesResponses<IEnumerable<ShelterDTO>>> GetShelters() {
            var response = new ServicesResponses<IEnumerable<ShelterDTO>>();
            try {
                var result = await _unitOfWork._shelterRepo.GetAllAsync();
                if (result != null) {
                    var mapper = _mapper.Map<IEnumerable<ShelterDTO>>(result);
                    response.Success = true;
                    response.Message = "Retrieved data successfully";
                    response.Data = mapper;


                }
                else {
                    response.Success = false;
                    response.Message = "Failed to Retrieved Data";
                }

            }
            catch (Exception ex) {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServicesResponses<IEnumerable<ShelterDTO>>> ListAllShelters() {
            var response = new ServicesResponses<IEnumerable<ShelterDTO>>();
            try {
                Task<IEnumerable<Shelter>> shelters = _unitOfWork._shelterRepo.GetAllSheltersAsync();

                var dtos = _mapper.Map<IEnumerable<ShelterDTO>>(shelters.Result);


                response.Message = "List of shelters";
                response.Success = true;
                response.Data = dtos;
            }
            catch (Exception e) {
                response.Success = false;
                response.Message = e.Message;
            }
            return response;
        }

        public async Task<ServicesResponses<ShelterDTO>> CreateShelterAsync(ShelterDTO shelterDto) {
            var response = new ServicesResponses<ShelterDTO>();
            try {
                shelterDto.Id = null;
                var shelter = _mapper.Map<Shelter>(shelterDto);
                await _unitOfWork._shelterRepo.AddAsync(shelter);
                await _unitOfWork.SaveChangeAsync();

                response.Data = _mapper.Map<ShelterDTO>(shelter);
                response.Success = true;
                response.Message = "Shelter created successfully";

            }
            catch (Exception ex) {
                response.Success = false;
                response.Message = ex.Message;

            }
            return response;

        }
        public async Task<ServicesResponses<ShelterDTO>> UpdateShelterAsync(ShelterDTO shelterDto) {
            var response = new ServicesResponses<ShelterDTO>();
            if (shelterDto.Id == null) {
                response.Success = false;
                response.Message = "Id is required";
                return response;
            }
            try {
                var shelter = await _unitOfWork._shelterRepo.GetByIdAsync(shelterDto.Id.Value);
                if (shelter == null) {
                    response = null;
                }
                else {
                    _mapper.Map(shelterDto, shelter);
                    _unitOfWork._shelterRepo.Update(shelter);
                    await _unitOfWork.SaveChangeAsync();

                    response.Data = shelterDto;
                    response.Success = true;
                }
            }
            catch (Exception e) {
                response.Success = false;
                response.Message = e.Message;
            }
            return response;
        }

        public async Task<bool> DeleteShelterAsync(Guid id) {
            var shelter = await _unitOfWork._shelterRepo.GetByIdAsync(id);
            if (shelter == null) {
                return false;
            }

            _unitOfWork._shelterRepo.Delete(shelter);
            await _unitOfWork.SaveChangeAsync();
            return true;
        }


    }
}
