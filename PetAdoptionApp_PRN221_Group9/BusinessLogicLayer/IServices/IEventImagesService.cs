using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogicLayer.ViewModels.EventDTO;
using BusinessLogicLayer.ViewModels.PetImageDTOs;
using CloudinaryDotNet.Actions;
using DataAccessObjects.ServicesResponses;
using Microsoft.AspNetCore.Http;

namespace BusinessLogicLayer.IServices {
    public interface IEventImagesService {

        public Task<ServicesResponses<ImageUploadResult>> AddPhoto(IFormFile file, Guid PetId);
        public Task<ServicesResponses<IEnumerable<EventImageDTO>>> GetEventImagesById(Guid Id);
        public Task<ServicesResponses<DeletionResult>> DeletePhotos(Guid photoId);

    }
}
