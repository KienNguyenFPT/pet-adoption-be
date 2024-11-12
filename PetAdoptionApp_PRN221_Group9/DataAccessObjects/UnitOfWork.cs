using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogicLayer;
using BusinessLogicLayer.IRepositories;

namespace DataAccessObjects {
    public class UnitOfWork : IUnitOfWork {
        private readonly IShelterRepo ShelterRepo;
        private readonly IUserRepo UserRepo;
        private readonly AppDBContext _appDbContext;
        private readonly IPetRepo PetRepo;
        private readonly IPetImageRepo PetImageRepo;
        private readonly IEventRepo EventRepo;
        private readonly IAdoptionRepo AdoptionRepo;
        private readonly IDonationRepo DonationRepo;
        private readonly IEventImageRepo EventImageRepo;
        private readonly IEventEnrollmentRepo EventEnrollmentRepo;

        public UnitOfWork(IShelterRepo shelterRepo, IUserRepo userRepo, AppDBContext appDbContext, IPetRepo petRepo, IPetImageRepo petImageRepo, IEventRepo eventRepo
            , IAdoptionRepo adoptionRepo, IDonationRepo donationRepo, IEventImageRepo eventImageRepo, IEventEnrollmentRepo eventEnrollmentRepo) {
            ShelterRepo = shelterRepo;
            UserRepo = userRepo;
            EventRepo = eventRepo;
            _appDbContext = appDbContext;
            PetRepo = petRepo;
            PetImageRepo = petImageRepo;
            AdoptionRepo = adoptionRepo;
            DonationRepo = donationRepo;
            EventImageRepo = eventImageRepo;
            EventEnrollmentRepo = eventEnrollmentRepo;
        }

        public IEventRepo _eventRepo => EventRepo;

        public IShelterRepo _shelterRepo => ShelterRepo;

        public IUserRepo _userRepo => UserRepo;

        public IPetRepo _petRepo => PetRepo;

        public IPetImageRepo _petImageRepo => PetImageRepo;

        public IAdoptionRepo _adoptionRepo => AdoptionRepo;

        public IDonationRepo _donationRepo => DonationRepo;

        public IEventImageRepo _eventImageRepo => EventImageRepo;

        public IEventEnrollmentRepo _eventEnrollmentRepo => EventEnrollmentRepo;

        public async Task<int> SaveChangeAsync() => await _appDbContext.SaveChangesAsync();

    }
}
