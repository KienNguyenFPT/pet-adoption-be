using BusinessLogicLayer.ViewModels.DonationDTOs;
using DataAccessObjects.ServicesResponses;
using PayPalHttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.IServices
{
    public interface IDonationServices
    {
        public Task<ServicesResponses<string>> CreateDonationPayments(DonationDTOs donationDTOs, Guid ShelterId);
        public Task<ServicesResponses<DonationDTOs>> CaptureDonationPayment(string transactionId);

        public Task<ServicesResponses<IEnumerable<GetDonationDTO>>> GetAllDonationByUserId(Guid UserId);

        public Task<ServicesResponses<GetDonationDTO>> GetDonationById(Guid Id);

        public Task<ServicesResponses<IEnumerable<GetDonationDTO>>> GetAllDonations();



        public Task<ServicesResponses<GetDonationDTO>> GetDonationByUserIdWithPaymentStatus(Guid Id);



        public Task<ServicesResponses<DonationDTOs>> CaptureDonationPayment2ndVersion(string transactionId);

        

        /*public Task<DonationDTOs?> GetDonationByTransactionId(string transactionId);*/
    }
}
