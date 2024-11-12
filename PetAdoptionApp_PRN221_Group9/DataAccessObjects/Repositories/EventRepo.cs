using BusinessLogicLayer.IRepositories;
using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace DataAccessObjects.Repositories {
    public class EventRepo : GenericRepository<Event>, IEventRepo {

        private readonly AppDBContext _dbContext;
        public EventRepo(AppDBContext dbContext) : base(dbContext) {

            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Event>> GetEventsWithRelationship() {
            try {
                var result = await _dbContext.Events.Include(x => x.Images).Include(x => x.Enrollments).ToListAsync();
                if (result.Any()) {
                    return result;
                } else {
                    return new List<Event>();
                }
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Event> GetEventWithRelationshipById(Guid id) {
            try {
                var result = await _dbContext.Events.Include(x => x.Images).Include(x => x.Enrollments).FirstOrDefaultAsync(x => x.Id == id);
                if (result != null) {
                    return result;
                } else {
                    return new Event();
                }
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Event>> GetEventOfUser(Guid userId) {
            var result = new List<Event>();
            var enroll = await _dbContext.Enrollments.Where(e => e.UserId == userId).Include(e => e.Event).ToListAsync();
            foreach (var item in enroll) {
                result.Add(item.Event);
            }
            return result;
        }
    }
    public class EventEnrollmentRepo : GenericRepository<Enrollment>, IEventEnrollmentRepo {

        private readonly AppDBContext _dbContext;
        public EventEnrollmentRepo(AppDBContext dbContext) : base(dbContext) {

            _dbContext = dbContext;
        }

        public async Task<Enrollment> GetEnrollmentAsync(Guid eventId, Guid userId) {
            return await _dbContext.Enrollments.FirstOrDefaultAsync<Enrollment>(e => e.EventId == eventId && e.UserId == userId);
        }

        public async Task<IEnumerable<Enrollment>> GetUserEnrollmentsAsync(Guid userId)
        {
            return await _dbContext.Set<Enrollment>()
          .Where(e => e.UserId == userId)
          .Include(e => e.Event) // Ensure related Event entity is included
          .ToListAsync();
        }
    }
}
