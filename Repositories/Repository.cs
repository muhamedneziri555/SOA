using MongoDB.Driver;
using System.Linq.Expressions;
using EventMngt.Data;

namespace EventMngt.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly IMongoCollection<T> _collection;

    public Repository(MongoDbContext context, string collectionName)
    {
        _collection = context.GetCollection<T>(collectionName);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

    public virtual async Task<T?> GetByIdAsync(string id)
    {
        var filter = Builders<T>.Filter.Eq("_id", id);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public virtual async Task<IEnumerable<T>> FindAsync(FilterDefinition<T> filter)
    {
        return await _collection.Find(filter).ToListAsync();
    }

    public virtual async Task<T?> FindOneAsync(FilterDefinition<T> filter)
    {
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public virtual async Task AddAsync(T entity)
    {
        await _collection.InsertOneAsync(entity);
    }

    public virtual async Task UpdateAsync(T entity)
    {
        var id = GetIdValue(entity);
        var filter = Builders<T>.Filter.Eq("_id", id);
        await _collection.ReplaceOneAsync(filter, entity);
    }

    public virtual async Task DeleteAsync(string id)
    {
        var filter = Builders<T>.Filter.Eq("_id", id);
        await _collection.DeleteOneAsync(filter);
    }

    protected string GetIdValue(T entity)
    {
        var property = typeof(T).GetProperty("Id");
        if (property == null)
            throw new InvalidOperationException("Entity must have an Id property");

        var value = property.GetValue(entity);
        if (value == null)
            throw new InvalidOperationException("Entity Id cannot be null");

        return value.ToString()!;
    }

    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _collection.Find(predicate).ToListAsync();
    }

    public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
    {
        return await _collection.Find(predicate).AnyAsync();
    }

    public virtual async Task<long> CountAsync(Expression<Func<T, bool>> predicate)
    {
        return await _collection.CountDocumentsAsync(predicate);
    }
} 