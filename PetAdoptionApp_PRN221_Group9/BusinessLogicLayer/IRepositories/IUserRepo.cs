using BusinessObjects;
using BusinessObjects.Enum;
using CloudinaryDotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.IRepositories
{
    public interface IUserRepo : IGenericRepository<User>
    {
        public Task<User> LoginAccountByEmailAddressAndPassword(string emailAddress, string password);
        public Task<User> RegisterAccount(User account);
        public Task<bool> CheckEmailExists(string emailAddress);
        public Task<bool> CheckPhoneNumberExists(string phoneNumber);
        public Task<User> GetUserByConfirmationToken(string token);
        public Task<IEnumerable<User>> SearchByNameAsync(string name);
        public Task<IEnumerable<User>> SearchByRoleNameAsync(Role roleName);
        
    }
}
