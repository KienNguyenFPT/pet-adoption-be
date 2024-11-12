using AutoMapper;
using BusinessLogicLayer.Commons;
using BusinessLogicLayer.IServices;
using BusinessLogicLayer.Utils;
using BusinessLogicLayer.ViewModels.UserDTOs;
using BusinessObjects;
using BusinessObjects.Enum;
using CloudinaryDotNet;
using DataAccessObjects;
using DataAccessObjects.ServicesResponses;
using DataAccessObjects.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class AuthenticationServices : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentTimeServices _timeServices;
        private readonly IClaimServices _claimServices; //for checking user has logged in or not
        private readonly AppConfiguration _appConfiguration;


        public AuthenticationServices(IUnitOfWork unitOfWork, IMapper mapper, ICurrentTimeServices currentTimeServices,
            IClaimServices claimServices, IOptions<AppConfiguration> appConfiguration)
        {
           
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _timeServices = currentTimeServices;
            _claimServices = claimServices;
            _appConfiguration = appConfiguration.Value ?? throw new ArgumentNullException(nameof(appConfiguration)); 
        }



        //For Azure

        public async Task<ServicesResponses<RegistrationDTOs>> CreateAdminAccountOnAzure(RegistrationDTOs registrationDTOs)
        {
            var response = new ServicesResponses<RegistrationDTOs>();
            try
            {

                var mapper = _mapper.Map<User>(registrationDTOs);
                var checkEmail = await _unitOfWork._userRepo.CheckEmailExists(mapper.EmailAddress);
                if (checkEmail)
                {
                    response.Success = true;
                    response.Message = "Email is already created";
                    return response;
                }
                mapper.PasswordHash = Utils.HashPassword.HashWithSHA256(mapper.PasswordHash);
                mapper.ConfirmationToken = Guid.NewGuid().ToString(); //create token for confirmation
                mapper.Status = 1;
                mapper.Role = Role.Administrator;
                await _unitOfWork._userRepo.AddAsync(mapper);



                var confirmationLink = $"https://petadoptionsecondapi-ekaxg2c0ccg3fthq.southeastasia-01.azurewebsites.net/swagger/confirm?token={mapper.ConfirmationToken}";
                var emailSent = await SendEmails.SendConfirmationEmail(mapper.EmailAddress, confirmationLink);
                if (!emailSent)
                {
                    response.Success = false;
                    response.Message = "Error sending confirmation email.";
                    return response;
                }
                else
                {
                    var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                    if (isSuccess)
                    {
                        var accountDto = _mapper.Map<RegistrationDTOs>(mapper);
                        response.Success = true;
                        response.Message = "Registered Successfully";
                        response.Data = accountDto;
                        return response;
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Error saving the account";
                    }


                }

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;
        }

        public async Task<ServicesResponses<RegistrationDTOs>> CreateStaffccountOnAzure(RegistrationDTOs registrationDTOs)
        {
            var response = new ServicesResponses<RegistrationDTOs>();
            try
            {

                var mapper = _mapper.Map<User>(registrationDTOs);
                var checkEmail = await _unitOfWork._userRepo.CheckEmailExists(mapper.EmailAddress);
                if (checkEmail)
                {
                    response.Success = true;
                    response.Message = "Email is already created";
                    return response;
                }
                mapper.PasswordHash = Utils.HashPassword.HashWithSHA256(mapper.PasswordHash);
                mapper.ConfirmationToken = Guid.NewGuid().ToString(); //create token for confirmation
                mapper.Status = 1;
                mapper.Role = Role.Staff;
                await _unitOfWork._userRepo.AddAsync(mapper);



                var confirmationLink = $"https://petadoptionsecondapi-ekaxg2c0ccg3fthq.southeastasia-01.azurewebsites.net/swagger/confirm?token={mapper.ConfirmationToken}";
                var emailSent = await SendEmails.SendConfirmationEmail(mapper.EmailAddress, confirmationLink);
                if (!emailSent)
                {
                    response.Success = false;
                    response.Message = "Error sending confirmation email.";
                    return response;
                }
                else
                {
                    var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                    if (isSuccess)
                    {
                        var accountDto = _mapper.Map<RegistrationDTOs>(mapper);
                        response.Success = true;
                        response.Message = "Registered Successfully";
                        response.Data = accountDto;
                        return response;
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Error saving the account";
                    }


                }

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;
        }

        public async Task<ServicesResponses<RegistrationDTOs>> CreateUserAccountOnAzure(RegistrationDTOs registrationDTOs)
        {
            var response = new ServicesResponses<RegistrationDTOs>();
            try
            {
                var mapper = _mapper.Map<User>(registrationDTOs);
                var checkEmail = await _unitOfWork._userRepo.CheckEmailExists(mapper.EmailAddress);
                if (checkEmail)
                {
                    response.Success = true;
                    response.Message = "Email is already created";
                    return response;
                }
                mapper.PasswordHash = Utils.HashPassword.HashWithSHA256(mapper.PasswordHash);
                mapper.ConfirmationToken = Guid.NewGuid().ToString(); //create token for confirmation
                mapper.Status = 1;
                mapper.Role = Role.User;
                await _unitOfWork._userRepo.AddAsync(mapper);



                var confirmationLink = $"https://petadoptionsecondapi-ekaxg2c0ccg3fthq.southeastasia-01.azurewebsites.net/swagger/confirm?token={mapper.ConfirmationToken}";
                var emailSent = await SendEmails.SendConfirmationEmail(mapper.EmailAddress, confirmationLink);
                if (!emailSent)
                {
                    response.Success = false;
                    response.Message = "Error sending confirmation email.";
                    return response;
                }
                else
                {
                    var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                    if (isSuccess)
                    {
                        var accountDto = _mapper.Map<RegistrationDTOs>(mapper);
                        response.Success = true;
                        response.Message = "Registered Successfully";
                        response.Data = accountDto;
                        return response;
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Error saving the account";
                    }


                }

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;
        }


        //For local host


        public async Task<ServicesResponses<string>> LoginAccountService(AuthenticationDTOs authentication)
        {
            var response = new ServicesResponses<string>();
            try
            {
                var hashedPassword = Utils.HashPassword.HashWithSHA256(authentication.PasswordHash);
                var result = await _unitOfWork._userRepo.LoginAccountByEmailAddressAndPassword(authentication.EmailAddress, hashedPassword);
                
                if (result == null)
                {
                    response.Success = false;
                    response.Message = "Invalid email or password";
                    return response;
                }
                if (result.ConfirmationToken != null && !result.IsConfirmed)
                {
                    response.Success = false;
                    response.Message = "Please confirm via link in your email";
                    return response;
                }


                // Access JWT Section



                var token = result.GenerateJsonWebToken(
                    _appConfiguration,
                    _appConfiguration.JWTSection.SecretKey,
                    _timeServices.GetCurrentTime()
                );
                /*var refresh = GenerateJsonWebTokenString.GenerateRefreshToken();
                result.RefreshToken = refresh;
                result.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(30);
                _unitOfWork._userRepo.Update(result);
                await _unitOfWork.SaveChangeAsync();
                */
                


                response.Success = true;
                response.Message = "Login Successfully";
                response.Data = token;
                /*response.RefreshToken = refresh;*/

            }
            catch (DbException ex)
            {
                response.Success = false;
                response.Message = "Database error occurred.";
                response.ErrorMessages = new List<string> { ex.Message };
            }


            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;


        }

        public async Task<ServicesResponses<RegistrationDTOs>> RegisterAccountService(RegistrationDTOs registrationDTOs)
        {
            var response = new ServicesResponses<RegistrationDTOs>();
            try
            {

                var mapper = _mapper.Map<User>(registrationDTOs);
                var checkEmail = await _unitOfWork._userRepo.CheckEmailExists(mapper.EmailAddress);
                if (checkEmail)
                {
                    response.Success = true;
                    response.Message = "Email is already created";
                    return response;
                }
                mapper.PasswordHash = Utils.HashPassword.HashWithSHA256(mapper.PasswordHash);
                mapper.ConfirmationToken = Guid.NewGuid().ToString(); //create token for confirmation
                mapper.Status = 1;
                mapper.Role = Role.User;
                await _unitOfWork._userRepo.AddAsync(mapper);



                var confirmationLink = $"https://localhost:5001/swagger/confirm?token={mapper.ConfirmationToken}";
                var emailSent = await SendEmails.SendConfirmationEmail(mapper.EmailAddress, confirmationLink);
                if (!emailSent)
                {
                    response.Success = false;
                    response.Message = "Error sending confirmation email.";
                    return response;
                }else
                {
                    var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                    if(isSuccess)
                    {
                        var accountDto = _mapper.Map<RegistrationDTOs>(mapper);
                        response.Success = true;
                        response.Message = "Registered Successfully";
                        response.Data = accountDto;
                        return response;
                    }else
                    {
                        response.Success = false;
                        response.Message = "Error saving the account";
                    }
                
                
                }

            } catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;
        }

        public async Task<ServicesResponses<RegistrationDTOs>> RegisterAsAdministrator(RegistrationDTOs registrationDTOs)
        {
            var response = new ServicesResponses<RegistrationDTOs>();
            try
            {

                var mapper = _mapper.Map<User>(registrationDTOs);
                var checkEmail = await _unitOfWork._userRepo.CheckEmailExists(mapper.EmailAddress);
                if (checkEmail)
                {
                    response.Success = true;
                    response.Message = "Email is already created";
                    return response;
                }
                mapper.PasswordHash = Utils.HashPassword.HashWithSHA256(mapper.PasswordHash);
                mapper.ConfirmationToken = Guid.NewGuid().ToString(); //create token for confirmation
                mapper.Status = 1;
                mapper.Role = Role.Administrator;
                await _unitOfWork._userRepo.AddAsync(mapper);



                var confirmationLink = $"https://localhost:5001/swagger/confirm?token={mapper.ConfirmationToken}";
                var emailSent = await SendEmails.SendConfirmationEmail(mapper.EmailAddress, confirmationLink);
                if (!emailSent)
                {
                    response.Success = false;
                    response.Message = "Error sending confirmation email.";
                    return response;
                }
                else
                {
                    var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                    if (isSuccess)
                    {
                        var accountDto = _mapper.Map<RegistrationDTOs>(mapper);
                        response.Success = true;
                        response.Message = "Registered Successfully";
                        response.Data = accountDto;
                        return response;
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Error saving the account";
                    }
                }

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;
        }

        public async Task<ServicesResponses<RegistrationDTOs>> RegisterAsStaff(RegistrationDTOs registrationDTOs)
        {
            var response = new ServicesResponses<RegistrationDTOs>();
            try
            {

                var mapper = _mapper.Map<User>(registrationDTOs);
                var checkEmail = await _unitOfWork._userRepo.CheckEmailExists(mapper.EmailAddress);
                if (checkEmail)
                {
                    response.Success = true;
                    response.Message = "Email is already created";
                    return response;
                }
                mapper.PasswordHash = Utils.HashPassword.HashWithSHA256(mapper.PasswordHash);
                mapper.ConfirmationToken = Guid.NewGuid().ToString(); //create token for confirmation
                mapper.Status = 1;
                mapper.Role = Role.Staff;
                await _unitOfWork._userRepo.AddAsync(mapper);



                var confirmationLink = $"https://localhost:5001/swagger/confirm?token={mapper.ConfirmationToken}";
                var emailSent = await SendEmails.SendConfirmationEmail(mapper.EmailAddress, confirmationLink);
                if (!emailSent)
                {
                    response.Success = false;
                    response.Message = "Error sending confirmation email.";
                    return response;
                }
                else
                {
                    var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                    if (isSuccess)
                    {
                        var accountDto = _mapper.Map<RegistrationDTOs>(mapper);
                        response.Success = true;
                        response.Message = "Registered Successfully";
                        response.Data = accountDto;
                        return response;
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Error saving the account";
                    }


                }

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;
        }
    }
}
