using MongoDB.Driver;
using System.Linq.Expressions;

namespace EventMngt.Repositories;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(string id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(string id);
    Task<IEnumerable<T>> FindAsync(FilterDefinition<T> filter);
    Task<T?> FindOneAsync(FilterDefinition<T> filter);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    Task<long> CountAsync(Expression<Func<T, bool>> predicate);
} 