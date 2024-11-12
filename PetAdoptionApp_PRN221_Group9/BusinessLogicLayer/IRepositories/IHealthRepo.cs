using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.IRepositories
{
    public interface IHealthRepo
    {
        Task<Healths> GetHealthByIdAsync(Guid id);
        Task<IEnumerable<Healths>> GetAllHealthsAsync();
        Task AddHealthAsync(Healths health);
        Task UpdateHealthAsync(Healths health);
        Task DeleteHealthAsync(Guid id);
    }
}
