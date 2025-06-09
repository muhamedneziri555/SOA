using EventMngt.Models;
using EventMngt.DTOs;

namespace EventMngt.Services;

public interface IRegistrationService
{
    Task<IEnumerable<RegistrationDTO>> GetAllRegistrationsAsync();
    Task<RegistrationDTO?> GetRegistrationByIdAsync(string id);
    Task<IEnumerable<RegistrationDTO>> GetRegistrationsByEventAsync(string eventId);
    Task<IEnumerable<RegistrationDTO>> GetRegistrationsByUserAsync(string userId);
    Task<RegistrationDTO> CreateRegistrationAsync(CreateRegistrationDTO registrationDto);
    Task<bool> UpdateRegistrationStatusAsync(string id, RegistrationStatus status);
    Task<bool> DeleteRegistrationAsync(string id);
} 