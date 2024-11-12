using BusinessLogicLayer.ViewModels.HealthDTO;
using DataAccessObjects.ServicesResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.IServices
{
    public interface IHealthServices
    {
        Task<ServicesResponses<HealthDTOs>> GetHealthById(Guid id);
        Task<ServicesResponses<IEnumerable<HealthDTOs>>> GetAllHealths();
        Task<ServicesResponses<HealthDTOs>> CreateHealth(HealthDTOs healthDTOs);
        Task<ServicesResponses<HealthDTOs>> UpdateHealth(HealthDTOs healthDTOs, Guid id);
        Task<ServicesResponses<bool>> DeleteHealth(Guid id);
    }
}
