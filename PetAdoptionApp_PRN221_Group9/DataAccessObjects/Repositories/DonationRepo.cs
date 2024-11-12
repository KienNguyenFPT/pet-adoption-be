using BusinessLogicLayer.IRepositories;
using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects.Repositories
{
    public class DonationRepo : GenericRepository<Donation>, IDonationRepo
    {
        private readonly AppDBContext _dbContext;


        public DonationRepo(AppDBContext dBContext) : base(dBContext) 
        {
            _dbContext = dBContext;
        }

        public async Task<IEnumerable<Donation>> GetDonationByUserId(Guid UserId)
        {
            try
            {
                var result = await _dbContext.Donations.Where(x => x.UserId == UserId).ToListAsync();
                if (result.Any())
                {
                    return result;
                }else
                {
                    return new List<Donation>();
                }



            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Donation> GetDonationByUserIDWithPaymentStatus(Guid UserId)
        {
            try
            {
                var result = await _dbContext.Donations.FirstOrDefaultAsync(x => x.UserId == UserId);
                if (result != null)
                {
                    return result;
                }
                else
                {
                    return new Donation();
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Donation> GetTransactionId(string id)
        {
            try
            {
                var result = await _dbContext.Donations.FirstOrDefaultAsync(x => x.TransactionId == id);
                if (result != null)
                {
                    return result;
                } else
                {
                    return new Donation();
                }



            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
