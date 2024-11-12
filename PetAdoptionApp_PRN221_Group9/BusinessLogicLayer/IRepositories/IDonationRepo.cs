using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.IRepositories
{
    public interface IDonationRepo : IGenericRepository<Donation>
    {
        public Task<Donation> GetTransactionId(string  id);
        public Task<IEnumerable<Donation>> GetDonationByUserId(Guid UserId);
        public Task<Donation> GetDonationByUserIDWithPaymentStatus(Guid UserId);
    }
}
