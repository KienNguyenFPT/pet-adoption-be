using BusinessLogicLayer.IRepositories;
using BusinessObjects;
using Microsoft.EntityFrameworkCore;


namespace DataAccessObjects.Repositories
{
    public class ShelterRepo : GenericRepository<Shelter>, IShelterRepo
    {
        private readonly AppDBContext _dbContext;
        public ShelterRepo(AppDBContext dbContext): base(dbContext)
        {

            _dbContext = dbContext;
        }
        
        public async Task<IEnumerable<Shelter>> GetAllSheltersAsync() {
            return await _dbContext.Shelters.ToListAsync();
        }

        public void Delete(Shelter entity) {
            _dbContext.Shelters.Remove(entity);
        }
    }
}
