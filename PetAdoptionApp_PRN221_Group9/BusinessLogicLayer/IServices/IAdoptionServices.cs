using BusinessLogicLayer.ViewModels.AdoptionsDTOs;
using DataAccessObjects.ServicesResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.IServices
{
    public interface IAdoptionServices
    {
        public Task<ServicesResponses<IEnumerable<GetAdoptionDTos>>> GetAllAdoptions();
        public Task<ServicesResponses<GetAdoptionDTos>> GetAdoption(Guid id);
        public Task<ServicesResponses<CreateAdoptionDTOs>> CreateAdoptions(CreateAdoptionDTOs adoptionDTOs, Guid PetId);
        public Task<ServicesResponses<AdoptionDTOs>> UpdateAdoptions(AdoptionDTOs adoptionDTOs, Guid Id);
        public Task<ServicesResponses<bool>> DeleteAdoption(Guid id);
        public Task<ServicesResponses<GetAdoptionDTos>> GetAdoptionByPetId(Guid PetId);


        public Task<ServicesResponses<IEnumerable<GetAdoptionDTos>>> GetAdoptionByUserId(Guid UserId);
    }
}
