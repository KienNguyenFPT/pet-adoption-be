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
    public class HealthRepo : IHealthRepo
    {
        private readonly AppDBContext _context;

        public HealthRepo(AppDBContext context)
        {
            _context = context;
        }

        public async Task<Healths> GetHealthByIdAsync(Guid id)
        {
            return await _context.Healths.FindAsync(id);
        }

        public async Task<IEnumerable<Healths>> GetAllHealthsAsync()
        {
            return await _context.Healths.ToListAsync();
        }

        public async Task AddHealthAsync(Healths health)
        {
            await _context.Healths.AddAsync(health);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateHealthAsync(Healths health)
        {
            _context.Healths.Update(health);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteHealthAsync(Guid id)
        {
            var health = await _context.Healths.FindAsync(id);
            if (health != null)
            {
                _context.Healths.Remove(health);
                await _context.SaveChangesAsync();
            }
        }
    }
}
