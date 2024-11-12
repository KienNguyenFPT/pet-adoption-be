using BusinessLogicLayer.IRepositories;
using BusinessObjects;
using BusinessObjects.Enum;
using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects.Repositories
{
    public class UserRepo : GenericRepository<User>, IUserRepo
    {
        private readonly AppDBContext _dbContext;


        public UserRepo(AppDBContext dBContext): base(dBContext) 
        {
            _dbContext = dBContext;
        }




        public async Task<bool> CheckEmailExists(string emailAddress)
        {
            try
            {
                var result = await _dbContext.Users.AnyAsync(x => x.EmailAddress == emailAddress);
                if (result)
                {
                    return true;
                }
                else
                {
                    return false;
                }




            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> CheckPhoneNumberExists(string phoneNumber)
        {
            try
            {
                var result = await _dbContext.Users.AnyAsync(x => x.PhoneNumber == phoneNumber);
                if (result)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task<User> GetUserByConfirmationToken(string token)
        {
            return await _dbContext.Users.SingleOrDefaultAsync(
            u => u.ConfirmationToken == token
            );
        }

        public async Task<User> LoginAccountByEmailAddressAndPassword(string emailAddress, string password)
        {
            try
            {
                var result = await _dbContext.Users.FirstOrDefaultAsync(x => x.EmailAddress == emailAddress && x.PasswordHash == password);
                if (result == null)
                {
                    return null;
                }
                else
                {
                    return result;
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<User> RegisterAccount(User account)
        {
            try
            {
                var result = await _dbContext.Users.AddAsync(account);
                if (result != null)
                {
                    return new User();
                }
                else
                {
                    return account;
                }




            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<User>> SearchByNameAsync(string name)
        {
            try
            {
                var result = await _dbContext.Users.Where(x => x.FullName.Contains(name)).ToListAsync();
                if (result != null)
                {
                    return result;
                }
                else
                {
                    return new List<User>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<User>> SearchByRoleNameAsync(Role roleName)
        {
            try
            {
                var result = await _dbContext.Users.Where(x => x.Role.Equals(roleName)).ToListAsync();
                if (result != null)
                {
                    return result;
                }
                else
                {
                    return new List<User>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
