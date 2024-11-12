using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class Donation : BaseEntity
    {
        public float? Money { get; set; }
        public DateTime? Date { get; set; }

        // Payment fields
        public string? TransactionId { get; set; }  // Store transaction ID from PayPal
        public string? PaymentStatus { get; set; }



        //Relationship
        public Guid? UserId {  get; set; }
        public virtual User? User { get; set; }
        public Guid? ShelterId { get; set; }
        public virtual Shelter? Shelter { get; set; }
    }
}
