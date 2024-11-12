using BusinessLogicLayer.ViewModels.PetDTOs;
using CloudinaryDotNet.Actions;
using DataAccessObjects.ServicesResponses;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.IServices
{
    public interface IPetServices
    {
        public Task<ServicesResponses<IEnumerable<GetPetDTOs>>> GetAllPets();
        public Task<ServicesResponses<GetPetDTOs>> GetPetsById(Guid id);
        public Task<ServicesResponses<PetDTOs>> CreatePets(PetDTOs petDTOs);
        public Task<ServicesResponses<PetDTOs>> UpdatePets(PetDTOs petDTOs, Guid petId);
        public Task<ServicesResponses<bool>> DeletePets(Guid petId);
        
    }
}
