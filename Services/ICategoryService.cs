using EventMngt.Models;
using EventMngt.DTOs;

namespace EventMngt.Services;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync();
    Task<CategoryDTO?> GetCategoryByIdAsync(string id);
    Task<CategoryDTO> CreateCategoryAsync(CreateCategoryDTO categoryDto);
    Task<bool> UpdateCategoryAsync(string id, UpdateCategoryDTO categoryDto);
    Task<bool> DeleteCategoryAsync(string id);
} 