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
    public class PetImageRepo : GenericRepository<PetImage>, IPetImageRepo
    {
        private readonly AppDBContext _dbContext;


        public PetImageRepo(AppDBContext appDBContext): base(appDBContext) 
        {
            _dbContext = appDBContext;
        }

        public async Task<PetImage> GetImageByID(Guid Id)
        {
            try
            {
                var result = await _dbContext.PetImages.Include(x => x.Pet).FirstOrDefaultAsync(x => x.PetId == Id);
                if (result == null)
                {
                    return new PetImage();
                }
                else
                {
                    return result;
                }



            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<PetImage>> GetImagesAsync()
        {
           try
            {
                var result = await _dbContext.PetImages.Include(x => x.Pet).ToListAsync();
                if (result == null)
                {
                    return new List<PetImage>();
                }else
                {
                    return result;
                }



            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<PetImage>> GetImagesById(Guid Id)
        {
            try
            {
                var result = await _dbContext.PetImages.Where(x => x.PetId == Id).ToListAsync();
                if (result == null)
                    return new List<PetImage>();
                else
                    return result;


            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
