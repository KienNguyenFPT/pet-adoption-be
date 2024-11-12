using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.IRepositories
{
    public interface IPetImageRepo : IGenericRepository<PetImage>
    {
        public Task<IEnumerable<PetImage>> GetImagesAsync();    
        public Task<PetImage> GetImageByID(Guid Id);    
        public Task<IEnumerable<PetImage>> GetImagesById(Guid Id);
        
    }
}
