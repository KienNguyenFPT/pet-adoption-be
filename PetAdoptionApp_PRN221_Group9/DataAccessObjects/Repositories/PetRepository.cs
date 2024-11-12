using BusinessLogicLayer.IRepositories;
using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects.Repositories
{
    public class PetRepository : GenericRepository<Pet>, IPetRepo
    {
        private readonly AppDBContext _appDBContext;

        public PetRepository(AppDBContext dbContext): base(dbContext) 
        {
            _appDBContext = dbContext;
        }

        public async Task<IEnumerable<Pet>> GetAllPetAndShelter()
        {
            try
            {
                var result = await _appDBContext.Pets.Include(x => x.Shelter).Include(x => x.PetImages).ToListAsync();
                if (result.Any())
                {
                    return result;
                }else
                {
                    return new List<Pet>();
                }
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Pet> GetPetById(Guid Id)
        {
            try
            {
                var result = await _appDBContext.Pets.Include(x => x.Shelter).Include(x => x.PetImages).FirstOrDefaultAsync(x => x.Id == Id);
                if (result != null)
                {
                    return result;
                } else
                {
                    return new Pet(); 
                }




            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
