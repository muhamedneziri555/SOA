using MongoDB.Driver;
using EventMngt.Data;
using EventMngt.Models;

namespace EventMngt.Repositories;

public interface IEventRepository : IRepository<Event>
{
    Task<IEnumerable<Event>> GetUpcomingEventsAsync();
    Task<IEnumerable<Event>> GetEventsByOrganizerAsync(string organizerId);
    Task<IEnumerable<Event>> GetEventsByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<long> GetRegistrationCountAsync(string eventId);
    Task<bool> IsEventFullAsync(string eventId);
}

public class EventRepository : MongoRepository<Event>, IEventRepository
{
    private readonly MongoDbContext _context;

    public EventRepository(MongoDbContext context) : base(context, "Events")
    {
        _context = context;
    }

    public async Task<IEnumerable<Event>> GetUpcomingEventsAsync()
    {
        var filter = Builders<Event>.Filter.And(
            Builders<Event>.Filter.Gt(e => e.StartDate, DateTime.UtcNow),
            Builders<Event>.Filter.Eq(e => e.IsActive, true)
        );
        return await _context.Events.Find(filter)
            .Sort(Builders<Event>.Sort.Ascending(e => e.StartDate))
            .ToListAsync();
    }

    public async Task<IEnumerable<Event>> GetEventsByOrganizerAsync(string organizerId)
    {
        return await _context.Events.Find(e => e.OrganizerId == organizerId)
            .Sort(Builders<Event>.Sort.Descending(e => e.CreatedAt))
            .ToListAsync();
    }

    public async Task<IEnumerable<Event>> GetEventsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var filter = Builders<Event>.Filter.And(
            Builders<Event>.Filter.Gte(e => e.StartDate, startDate),
            Builders<Event>.Filter.Lte(e => e.EndDate, endDate),
            Builders<Event>.Filter.Eq(e => e.IsActive, true)
        );
        return await _context.Events.Find(filter)
            .Sort(Builders<Event>.Sort.Ascending(e => e.StartDate))
            .ToListAsync();
    }

    public async Task<long> GetRegistrationCountAsync(string eventId)
    {
        return await _context.Registrations.CountDocumentsAsync(r => 
            r.EventId == eventId && r.Status == RegistrationStatus.Confirmed);
    }

    public async Task<bool> IsEventFullAsync(string eventId)
    {
        var @event = await GetByIdAsync(eventId);
        if (@event == null) return false;

        var registrationCount = await GetRegistrationCountAsync(eventId);
        return registrationCount >= @event.Capacity;
    }
} 