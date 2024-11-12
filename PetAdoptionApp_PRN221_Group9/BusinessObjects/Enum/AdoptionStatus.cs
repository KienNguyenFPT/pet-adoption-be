using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Enum
{
    public enum AdoptionStatus
    {
        Available,      // The pet is available for adoption.
        Pending,        // The pet's adoption is in progress (e.g., someone has shown interest but hasn't finalized).
        Adopted,        // The pet has been adopted.
        NotAvailable,   // The pet is currently not available for adoption (e.g., medical reasons).
        Hold,           // The pet is temporarily on hold (e.g., pending further assessment).
        Returned        // The pet was adopted but then returned to the shelter.
    }
}
