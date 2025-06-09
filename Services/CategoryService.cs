using MongoDB.Driver;
using EventMngt.Models;
using EventMngt.DTOs;
using EventMngt.Data;

namespace EventMngt.Services;

public class CategoryService : ICategoryService
{
    private readonly IMongoCollection<Category> _categories;

    public CategoryService(MongoDbContext context)
    {
        _categories = context.Categories;
    }

    public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync()
    {
        var categories = await _categories.Find(_ => true).ToListAsync();
        return categories.Select(c => new CategoryDTO
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description,
            CreatedAt = c.CreatedAt
        });
    }

    public async Task<CategoryDTO?> GetCategoryByIdAsync(string id)
    {
        var category = await _categories.Find(c => c.Id == id).FirstOrDefaultAsync();
        if (category == null) return null;

        return new CategoryDTO
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            CreatedAt = category.CreatedAt
        };
    }

    public async Task<CategoryDTO> CreateCategoryAsync(CreateCategoryDTO categoryDto)
    {
        var category = new Category
        {
            Name = categoryDto.Name,
            Description = categoryDto.Description,
            CreatedAt = DateTime.UtcNow
        };

        await _categories.InsertOneAsync(category);
        return new CategoryDTO
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            CreatedAt = category.CreatedAt
        };
    }

    public async Task<bool> UpdateCategoryAsync(string id, UpdateCategoryDTO categoryDto)
    {
        var update = Builders<Category>.Update
            .Set(c => c.Name, categoryDto.Name)
            .Set(c => c.Description, categoryDto.Description);

        var result = await _categories.UpdateOneAsync(c => c.Id == id, update);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteCategoryAsync(string id)
    {
        var result = await _categories.DeleteOneAsync(c => c.Id == id);
        return result.DeletedCount > 0;
    }
} 