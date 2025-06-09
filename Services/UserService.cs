using EventMngt.DTOs;
using EventMngt.Mappers;
using EventMngt.Models;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using System.Security.Claims;
using EventMngt.Exceptions;
using EventMngt.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EventMngt.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRepository<User> _userRepository;

    public UserService(UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, IRepository<User> userRepository)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _userRepository = userRepository;
    }

    public async Task<UserDto> GetByIdAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            throw new KeyNotFoundException($"User with ID {id} not found");

        return UserMapper.ToDto(user);
    }

    public async Task<UserDto> GetByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            throw new KeyNotFoundException($"User with email {email} not found");

        return UserMapper.ToDto(user);
    }

    public async Task<UserDto> GetByUsernameAsync(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
            throw new KeyNotFoundException($"User with username {username} not found");

        return UserMapper.ToDto(user);
    }

    public async Task<UserDto> CreateAsync(CreateUserDTO userDto)
    {
        var user = UserMapper.ToModel(userDto);
        var result = await _userManager.CreateAsync(user, userDto.Password);

        if (!result.Succeeded)
            throw new InvalidOperationException($"Failed to create user: {string.Join(", ", result.Errors.Select(e => e.Description))}");

        return UserMapper.ToDto(user);
    }

    public async Task<UserDto> UpdateAsync(string id, UpdateUserDTO userDto)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            throw new KeyNotFoundException($"User with ID {id} not found");

        UserMapper.UpdateModel(user, userDto);

        if (!string.IsNullOrEmpty(userDto.Password))
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, userDto.Password);
            if (!result.Succeeded)
                throw new InvalidOperationException($"Failed to update password: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
            throw new InvalidOperationException($"Failed to update user: {string.Join(", ", updateResult.Errors.Select(e => e.Description))}");

        return UserMapper.ToDto(user);
    }

    public async Task DeleteAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            throw new KeyNotFoundException($"User with ID {id} not found");

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
            throw new InvalidOperationException($"Failed to delete user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _userManager.Users.ToListAsync();
        return users.Select(UserMapper.ToDto);
    }

    public async Task<UserDto> GetCurrentUserAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
            throw new UnauthorizedAccessException("No HTTP context available");

        var user = await _userManager.GetUserAsync(httpContext.User);
        if (user == null)
            throw new UnauthorizedAccessException("User is not authenticated");

        return UserMapper.ToDto(user);
    }

    public async Task<UserDto> GetUserProfileAsync(ClaimsPrincipal principal)
    {
        var user = await _userManager.GetUserAsync(principal);
        if (user == null)
            throw new NotFoundException("User not found");

        return UserMapper.ToDto(user);
    }

    public async Task<UserDto> UpdateUserProfileAsync(ClaimsPrincipal principal, UpdateUserDTO userDto)
    {
        var user = await _userManager.GetUserAsync(principal);
        if (user == null)
            throw new NotFoundException("User not found");

        UserMapper.UpdateModel(user, userDto);
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
            throw new ValidationException(result.Errors.Select(e => e.Description));

        return UserMapper.ToDto(user);
    }
} 