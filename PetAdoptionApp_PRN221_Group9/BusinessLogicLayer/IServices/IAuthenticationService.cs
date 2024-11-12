using BusinessLogicLayer.ViewModels.UserDTOs;
using DataAccessObjects.ServicesResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.IServices
{
    public interface IAuthenticationService
    {
        public Task<ServicesResponses<RegistrationDTOs>> RegisterAccountService(RegistrationDTOs registrationDTOs);
        public Task<ServicesResponses<string>> LoginAccountService(AuthenticationDTOs authentication);
        public Task<ServicesResponses<RegistrationDTOs>> RegisterAsStaff(RegistrationDTOs registrationDTOs);
        public Task<ServicesResponses<RegistrationDTOs>> RegisterAsAdministrator(RegistrationDTOs registrationDTOs);


        //For Azure
        public Task<ServicesResponses<RegistrationDTOs>> CreateUserAccountOnAzure(RegistrationDTOs registrationDTOs);
        public Task<ServicesResponses<RegistrationDTOs>> CreateStaffccountOnAzure(RegistrationDTOs registrationDTOs);
        public Task<ServicesResponses<RegistrationDTOs>> CreateAdminAccountOnAzure(RegistrationDTOs registrationDTOs);


    }
}
