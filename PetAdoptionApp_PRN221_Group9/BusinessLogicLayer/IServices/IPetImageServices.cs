using BusinessLogicLayer.ViewModels.PetDTOs;
using BusinessLogicLayer.ViewModels.PetImageDTOs;
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
    public interface IPetImageServices
    {
        public Task<ServicesResponses<ImageUploadResult>> AddPhotos(IFormFile file, PetImagesDTOs pets, Guid PetId);
        public Task<ServicesResponses<IEnumerable<GetPetImageDTOs>>> GetPetImagesLists();
        public Task<ServicesResponses<GetPetImageDTOs>> GetPetImageById(Guid PetId);
        public Task<ServicesResponses<DeletionResult>> DeletePhotos(Guid photoId);
        public Task<ServicesResponses<IEnumerable<GetPetImageDTOs>>> GetAllPhotos();
        public Task<ServicesResponses<IEnumerable<GetPetImageDTOs>>> GetPetImagesById(Guid Id);
    }
}
