using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogicLayer.IRepositories;
using DataAccessObjects;

namespace BusinessLogicLayer {
    public interface IUnitOfWork {
        public IShelterRepo _shelterRepo { get; }
        public IUserRepo _userRepo { get; }
        public IPetRepo _petRepo { get; }
        public IPetImageRepo _petImageRepo { get; }
        public Task<int> SaveChangeAsync();
        public IEventRepo _eventRepo { get; }
        public IAdoptionRepo _adoptionRepo { get; }
        public IDonationRepo _donationRepo { get; }
        public IEventImageRepo _eventImageRepo { get; }
        public IEventEnrollmentRepo _eventEnrollmentRepo { get; }
    }
}
