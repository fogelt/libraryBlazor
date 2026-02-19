using System.Linq.Expressions;

namespace Library.Core.Interfaces;

public interface IGenericRepository<T> where T : class
{
  Task<IEnumerable<T>> GetAllAsync(params string[] includes);
  Task<T?> GetByIdAsync(string id);
  Task AddAsync(T entity);
  Task UpdateAsync(T entity);
  Task DeleteAsync(string id);
  Task<IEnumerable<T>> SearchAsync(Expression<Func<T, bool>> predicate);
}