using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
        public byte? Status { get; set; }
        
    }
}
