using BusinessLogicLayer.ViewModels.ShelterDTOs;
using DataAccessObjects.ServicesResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.IServices
{
    public interface IShelterServices
    {
        public Task<ServicesResponses<IEnumerable<ShelterDTO>>> GetShelters(); 
        
        public Task<ServicesResponses<IEnumerable<ShelterDTO>>> ListAllShelters();
        Task<ServicesResponses<ShelterDTO>> CreateShelterAsync(ShelterDTO shelterDto);
        Task<ServicesResponses<ShelterDTO>> UpdateShelterAsync(ShelterDTO shelterDto);
        Task<bool> DeleteShelterAsync(Guid id);
    }
}
