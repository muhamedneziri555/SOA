using EventMngt.DTOs;
using EventMngt.Models;
using MongoDB.Bson;

namespace EventMngt.Mappers;

public static class UserMapper
{
    public static UserDto ToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id.ToString(),
            Email = user.Email ?? string.Empty,
            UserName = user.UserName ?? string.Empty,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber ?? string.Empty,
            Role = user.Role,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            LastLoginAt = user.LastLoginAt
        };
    }

    public static User ToEntity(UserDto dto)
    {
        return new User
        {
            Id = ObjectId.Parse(dto.Id),
            Email = dto.Email,
            UserName = dto.UserName,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            PhoneNumber = dto.PhoneNumber,
            Role = dto.Role,
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt,
            LastLoginAt = dto.LastLoginAt
        };
    }

    public static UserProfileDTO ToProfileDTO(User user)
    {
        return new UserProfileDTO
        {
            Id = user.Id.ToString(),
            UserName = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            PhoneNumber = user.PhoneNumber ?? string.Empty,
            FirstName = user.FirstName,
            LastName = user.LastName,
            CreatedAt = user.CreatedAt,
            LastLoginAt = user.LastLoginAt
        };
    }

    public static User ToModel(CreateUserDTO dto)
    {
        return new User
        {
            Email = dto.Email,
            UserName = dto.UserName,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            PhoneNumber = dto.PhoneNumber,
            Role = UserRole.User
        };
    }

    public static void UpdateModel(User user, UpdateUserDTO dto)
    {
        if (dto.FirstName != null)
            user.FirstName = dto.FirstName;
        if (dto.LastName != null)
            user.LastName = dto.LastName;
        if (dto.PhoneNumber != null)
            user.PhoneNumber = dto.PhoneNumber;
    }
} 