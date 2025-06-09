using MongoDB.Driver;
using EventMngt.Models;
using EventMngt.DTOs;
using EventMngt.Data;

namespace EventMngt.Services;

public class RegistrationService : IRegistrationService
{
    private readonly IMongoCollection<Registration> _registrations;
    private readonly IEventService _eventService;

    public RegistrationService(MongoDbContext context, IEventService eventService)
    {
        _registrations = context.Registrations;
        _eventService = eventService;
    }

    public async Task<IEnumerable<RegistrationDTO>> GetAllRegistrationsAsync()
    {
        var registrations = await _registrations.Find(_ => true).ToListAsync();
        return registrations.Select(r => new RegistrationDTO
        {
            Id = r.Id,
            EventId = r.EventId,
            UserId = r.UserId,
            Status = r.Status,
            RegistrationDate = r.RegistrationDate
        });
    }

    public async Task<RegistrationDTO?> GetRegistrationByIdAsync(string id)
    {
        var registration = await _registrations.Find(r => r.Id == id).FirstOrDefaultAsync();
        if (registration == null) return null;

        return new RegistrationDTO
        {
            Id = registration.Id,
            EventId = registration.EventId,
            UserId = registration.UserId,
            Status = registration.Status,
            RegistrationDate = registration.RegistrationDate
        };
    }

    public async Task<IEnumerable<RegistrationDTO>> GetRegistrationsByEventAsync(string eventId)
    {
        var registrations = await _registrations.Find(r => r.EventId == eventId).ToListAsync();
        return registrations.Select(r => new RegistrationDTO
        {
            Id = r.Id,
            EventId = r.EventId,
            UserId = r.UserId,
            Status = r.Status,
            RegistrationDate = r.RegistrationDate
        });
    }

    public async Task<IEnumerable<RegistrationDTO>> GetRegistrationsByUserAsync(string userId)
    {
        var registrations = await _registrations.Find(r => r.UserId == userId).ToListAsync();
        return registrations.Select(r => new RegistrationDTO
        {
            Id = r.Id,
            EventId = r.EventId,
            UserId = r.UserId,
            Status = r.Status,
            RegistrationDate = r.RegistrationDate
        });
    }

    public async Task<RegistrationDTO> CreateRegistrationAsync(CreateRegistrationDTO registrationDto)
    {
        // Check if event is full
        if (await _eventService.IsEventFullAsync(registrationDto.EventId))
        {
            throw new InvalidOperationException("Event is full");
        }

        var registration = new Registration
        {
            EventId = registrationDto.EventId,
            UserId = registrationDto.UserId,
            Status = RegistrationStatus.Pending,
            RegistrationDate = DateTime.UtcNow
        };

        await _registrations.InsertOneAsync(registration);
        return new RegistrationDTO
        {
            Id = registration.Id,
            EventId = registration.EventId,
            UserId = registration.UserId,
            Status = registration.Status,
            RegistrationDate = registration.RegistrationDate
        };
    }

    public async Task<bool> UpdateRegistrationStatusAsync(string id, RegistrationStatus status)
    {
        var update = Builders<Registration>.Update
            .Set(r => r.Status, status);

        var result = await _registrations.UpdateOneAsync(r => r.Id == id, update);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteRegistrationAsync(string id)
    {
        var result = await _registrations.DeleteOneAsync(r => r.Id == id);
        return result.DeletedCount > 0;
    }
} 