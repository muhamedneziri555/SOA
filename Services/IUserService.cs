using EventMngt.DTOs;
using EventMngt.Models;

namespace EventMngt.Services;

public interface IUserService
{
    Task<UserDto> GetByIdAsync(string id);
    Task<UserDto> GetByEmailAsync(string email);
    Task<UserDto> GetByUsernameAsync(string username);
    Task<UserDto> CreateAsync(CreateUserDTO userDto);
    Task<UserDto> UpdateAsync(string id, UpdateUserDTO userDto);
    Task DeleteAsync(string id);
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<UserDto> GetCurrentUserAsync();
} 