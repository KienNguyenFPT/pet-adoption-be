using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace BusinessLogicLayer.IRepositories {
    public interface IEventImageRepo : IGenericRepository<EventImage> {

        public Task<IEnumerable<EventImage>> GetEventImagesById(Guid Id);

    }
}
