using BusinessObjects;

namespace BusinessLogicLayer.IRepositories {
    public interface IEventRepo : IGenericRepository<Event> {

        public Task<IEnumerable<Event>> GetEventsWithRelationship();
        public Task<Event> GetEventWithRelationshipById(Guid id);
        public Task<IEnumerable<Event>> GetEventOfUser(Guid userId);
    }

    public interface IEventEnrollmentRepo : IGenericRepository<Enrollment> {
        public Task<Enrollment> GetEnrollmentAsync(Guid eventId, Guid userId);


        public Task<IEnumerable<Enrollment>> GetUserEnrollmentsAsync(Guid userId);
    }
}
