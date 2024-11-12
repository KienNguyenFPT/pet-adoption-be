using AutoMapper;
using BusinessLogicLayer.IServices;
using BusinessLogicLayer.ViewModels.HealthDTO;
using BusinessObjects;
using DataAccessObjects.ServicesResponses;
using DataAccessObjects;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogicLayer.IRepositories;
using MimeKit.Cryptography;

namespace BusinessLogicLayer.Services
{
    public class HealthServices : IHealthServices
    {
        private readonly IHealthRepo _healthRepo;
        private readonly IMapper _mapper;
        private readonly ICurrentTimeServices _currentTimeServices;

        public HealthServices(IHealthRepo healthRepo, IMapper mapper, ICurrentTimeServices currentTimeServices)
        {
            _healthRepo = healthRepo;
            _mapper = mapper;
            _currentTimeServices = currentTimeServices;
        }

        public async Task<ServicesResponses<HealthDTOs>> GetHealthById(Guid id)
        {
            var health = await _healthRepo.GetHealthByIdAsync(id);
            if (health == null)
            {
                return new ServicesResponses<HealthDTOs> { Success = false, Message = "Health record not found." };
            }
            return new ServicesResponses<HealthDTOs> { Data = _mapper.Map<HealthDTOs>(health) };
        }

        public async Task<ServicesResponses<IEnumerable<HealthDTOs>>> GetAllHealths()
        {
            var healths = await _healthRepo.GetAllHealthsAsync();
            return new ServicesResponses<IEnumerable<HealthDTOs>> { Data = _mapper.Map<IEnumerable<HealthDTOs>>(healths) };
        }

        public async Task<ServicesResponses<HealthDTOs>> CreateHealth(HealthDTOs healthDTOs)
        {
            var health = _mapper.Map<Healths>(healthDTOs);
            health.Date = _currentTimeServices.GetCurrentTime();
            await _healthRepo.AddHealthAsync(health);
            return new ServicesResponses<HealthDTOs> { Data = _mapper.Map<HealthDTOs>(health) };
        }

        public async Task<ServicesResponses<HealthDTOs>> UpdateHealth(HealthDTOs healthDTOs, Guid id)
        {
            var health = await _healthRepo.GetHealthByIdAsync(id);
            if (health == null)
            {
                return new ServicesResponses<HealthDTOs> { Success = false, Message = "Health record not found." };
            }
            _mapper.Map(healthDTOs, health);
            await _healthRepo.UpdateHealthAsync(health);
            return new ServicesResponses<HealthDTOs> { Data = _mapper.Map<HealthDTOs>(health) };
        }

        public async Task<ServicesResponses<bool>> DeleteHealth(Guid id)
        {
            await _healthRepo.DeleteHealthAsync(id);
            return new ServicesResponses<bool> { Data = true };
        }
    }
}
