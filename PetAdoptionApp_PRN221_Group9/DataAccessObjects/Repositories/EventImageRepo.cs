using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogicLayer.IRepositories;
using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace DataAccessObjects.Repositories {
    public class EventImageRepo : GenericRepository<EventImage>, IEventImageRepo {

        private readonly AppDBContext _dbContext;

        public EventImageRepo(AppDBContext dbContext) : base(dbContext) {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<EventImage>> GetEventImagesById(Guid Id) {
            try {
                var result = await _dbContext.EventImages.Where(x => x.EventId == Id).ToListAsync();
                if (result == null)
                    return new List<EventImage>();
                else
                    return result;


            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }
    }
}
