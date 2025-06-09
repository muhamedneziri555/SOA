using EventMngt.DTOs;
using EventMngt.Models;

namespace EventMngt.Mappers;

public static class RegistrationMapper
{
    public static RegistrationDTO ToDTO(Registration model)
    {
        return new RegistrationDTO
        {
            Id = model.Id,
            EventId = model.EventId,
            UserId = model.UserId,
            Status = model.Status,
            RegistrationDate = model.RegistrationDate,
            UpdatedAt = model.UpdatedAt,
            Notes = model.Notes
        };
    }

    public static Registration ToModel(CreateRegistrationDTO dto)
    {
        return new Registration
        {
            EventId = dto.EventId,
            UserId = dto.UserId ?? string.Empty, // Get from current user if not provided
            Status = RegistrationStatus.Pending,
            RegistrationDate = DateTime.UtcNow,
            Notes = dto.Notes
        };
    }

    public static void UpdateModel(Registration model, UpdateRegistrationDTO dto)
    {
        model.Status = dto.Status;
        model.Notes = dto.Notes;
        model.UpdatedAt = DateTime.UtcNow;
    }
} 