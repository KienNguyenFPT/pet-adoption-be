using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.ViewModels.DonationDTOs
{
    public class GetDonationDTO
    {
        
        public float? Money { get; set; }
        public DateTime? Date { get; set; }
        public Guid? ShelterId { get; set; }
        public string? TransactionId { get; set; }
        public string? PaymentStatus { get; set; }
    }
}
