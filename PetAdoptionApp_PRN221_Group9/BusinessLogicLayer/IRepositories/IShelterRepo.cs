using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.IRepositories
{
    public interface IShelterRepo : IGenericRepository<Shelter>
    {
        Task<IEnumerable<Shelter>> GetAllSheltersAsync();
    }
}
