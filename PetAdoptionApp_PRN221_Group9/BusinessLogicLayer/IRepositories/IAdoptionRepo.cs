using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.IRepositories
{
    public interface IAdoptionRepo : IGenericRepository<Adoption>
    {
        public Task<Adoption> GetAdoptionByPetId(Guid petId);
        public Task<IEnumerable<Adoption>> GetAdoptionListByUser(Guid Id);
    }
}
