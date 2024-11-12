using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.IRepositories
{
    public interface IPetRepo : IGenericRepository<Pet>
    {
        public Task<IEnumerable<Pet>> GetAllPetAndShelter();
        public Task<Pet> GetPetById(Guid Id);
    }
}
