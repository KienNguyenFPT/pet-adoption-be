using BusinessLogicLayer.IRepositories;
using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        public DbSet<TEntity> _dbSet;

        public GenericRepository(AppDBContext dbContext)
        {
            _dbSet = dbContext.Set<TEntity>();
        }


        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Delete(TEntity entity) {
            _dbSet.Remove(entity);
        }
        public async Task AddRangeAsync(List<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(Guid id)
        {
            var result = await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
            return result;
        }

        public void SoftRemove(TEntity entity)
        {
            entity.Status = 0; 
            _dbSet.Update(entity);
        }

        public void SoftRemoveRange(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.Status = 0;
            }
            _dbSet.UpdateRange(entities);
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public void UpdateRange(List<TEntity> entities)
        {
            _dbSet.UpdateRange(entities.ToList());
        }

      /*  int pageNumber = 2;
        int pageSize = 10;

        var(items, totalCount) = await productRepository.GetPagedAsync(pageNumber, pageSize);

        // Calculate the total number of pages
        int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);*/
        public async Task<(List<TEntity> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
        {
            // Get total count of items
            int totalCount = await _dbSet.CountAsync();

            // Get the paged data
            var items = await _dbSet
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}
