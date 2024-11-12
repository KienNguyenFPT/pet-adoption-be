using AutoMapper;
using BusinessLogicLayer.Commons;
using BusinessLogicLayer.IServices;
using BusinessLogicLayer.ViewModels.PetDTOs;
using BusinessLogicLayer.ViewModels.PetImageDTOs;
using BusinessObjects;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DataAccessObjects.ServicesResponses;
using Docker.DotNet.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class PetImageServices : IPetImageServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IClaimServices _claimServices;
        private readonly ICurrentTimeServices _currentTimeServices;
        private readonly Cloudinary _cloud;

        public PetImageServices(IClaimServices claimServices, ICurrentTimeServices currentTimeServices, IMapper mapper, IUnitOfWork unitOfWork
            , IOptions<CloudinarySettings> config)
        {
            _claimServices = claimServices;

            _currentTimeServices = currentTimeServices;

            _mapper = mapper;
            _unitOfWork = unitOfWork;
            var cloud = new Account
            {
                Cloud = config.Value.CloudName,
                ApiKey = config.Value.ApiKey,
                ApiSecret = config.Value.ApiSecret,
            };
            _cloud = new Cloudinary(cloud);
        }

        public async Task<ServicesResponses<ImageUploadResult>> AddPhotos(IFormFile file, PetImagesDTOs petImagesDTOs, Guid petId)
        {
            var response = new ServicesResponses<ImageUploadResult>();

            try
            {
                var getUserId = _claimServices.GetCurrentUserId;
                if (getUserId == null)
                {
                    response.Success = false;
                    response.Message = "Please login first to use this function";
                    return response;
                }

                var pet = await _unitOfWork._petRepo.GetByIdAsync(petId);
                if (pet == null)
                {
                    response.Success = false;
                    response.Message = "Pet not found";
                    return response;
                }

                if (file == null || file.Length == 0)
                {
                    response.Success = false;
                    response.Message = "No file provided or file is empty";
                    return response;
                }

                // Optional: Validate the file format (if necessary)
                var validImageTypes = new[] { "image/jpeg", "image/png", "image/gif" };
                if (!validImageTypes.Contains(file.ContentType))
                {
                    response.Success = false;
                    response.Message = "Invalid file type. Only JPEG, PNG, and GIF are allowed.";
                    return response;
                }

                await using var stream = file.OpenReadStream();

                // Define the image upload parameters for Cloudinary
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
                };

                // Upload the image to Cloudinary
                var uploadResult = await _cloud.UploadAsync(uploadParams);

                // Log the upload result
                /*Console.WriteLine($"Cloudinary upload result: {uploadResult.StatusCode} - {uploadResult.Error?.Message}");*/

                if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    // Map the uploaded URL to the DTO
                    petImagesDTOs.Image = uploadResult.Url.ToString();

                    // Save the uploaded image URL and associated PetId to the database
                    var petImage = new PetImage
                    {
                        Image = petImagesDTOs.Image,
                        PetId = pet.Id
                    };

                    await _unitOfWork._petImageRepo.AddAsync(petImage);

                    // Save changes to the database
                    var isSaved = await _unitOfWork.SaveChangeAsync() > 0;
                    if (isSaved)
                    {
                        response.Success = true;
                        response.Message = "Image added successfully";
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Failed to save image to the database";
                    }
                }
                else
                {
                    response.Success = false;
                    response.Message = "Error uploading image: " + uploadResult.Error?.Message;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { "An error occurred while uploading the image: " + ex.Message };
            }

            return response;
        }

        public async Task<ServicesResponses<DeletionResult>> DeletePhotos(Guid photoId)
        {
            var response = new ServicesResponses<DeletionResult>();
            try
            {
                var getUser = _claimServices.GetCurrentUserId;
                if (getUser == null)
                {
                    response.Success = false;
                    response.Message = "Please login first to use this function";
                    return response;
                }
                var getPhotoId = await _unitOfWork._petImageRepo.GetByIdAsync(photoId);
                if (getPhotoId == null)
                {
                    response.Success = false;
                    response.Message = "Photo is not found";
                    return response;
                }
                

                string PhotostringId = photoId.ToString();

                var deleteParms = new DeletionParams(PhotostringId);
                var resultImage =  await _cloud.DestroyAsync(deleteParms);
                _unitOfWork._petImageRepo.Delete(getPhotoId);
                var IsSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (IsSuccess && resultImage != null)
                {
                    response.Success = true;
                    response.Message = "Delete PhotoSuccessfully";
                    return response;
                }else
                {
                    response.Success = false;
                    response.Message = "Failed to delete";
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

        public async Task<ServicesResponses<IEnumerable<GetPetImageDTOs>>> GetAllPhotos()
        {
            var response = new ServicesResponses<IEnumerable<GetPetImageDTOs>>();
            try
            {
                var result = await _unitOfWork._petImageRepo.GetAllAsync();
                if (result != null)
                {
                    response.Success = true;
                    response.Message = "Retrieved Data Succesfully";
                    response.Data = _mapper.Map<IEnumerable<GetPetImageDTOs>>(result);  
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

        public async Task<ServicesResponses<GetPetImageDTOs>> GetPetImageById(Guid PetId)
        {
            var response = new ServicesResponses<GetPetImageDTOs>();
            try
            {
                var result = await _unitOfWork._petImageRepo.GetImageByID(PetId);
                if (result != null)
                {
                    response.Success = true;
                    response.Message = "Retrieved Data Successfully";
                    response.Data = _mapper.Map<GetPetImageDTOs>(result);
                }else
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

        public async Task<ServicesResponses<IEnumerable<GetPetImageDTOs>>> GetPetImagesById(Guid Id)
        {
            var response = new ServicesResponses<IEnumerable<GetPetImageDTOs>>();
            try
            {
                var result = await _unitOfWork._petImageRepo.GetImagesById(Id);
                if (result != null)
                {
                    response.Success = true;
                    response.Message = "Retrieved Data Successfully";
                    response.Data = _mapper.Map<IEnumerable<GetPetImageDTOs>>(result);
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

        public async Task<ServicesResponses<IEnumerable<GetPetImageDTOs>>> GetPetImagesLists()
        {
            var response = new ServicesResponses<IEnumerable<GetPetImageDTOs>>();
            try
            {
                var result = await _unitOfWork._petImageRepo.GetImagesAsync();
                if (result != null)
                {
                    response.Success = true;
                    response.Message = "Retrieved Data Successfully";
                    response.Data = _mapper.Map<IEnumerable<GetPetImageDTOs>>(result);
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
    }
}
