using System.Linq.Expressions;
using Library.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Library.Data.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
  private readonly IDbContextFactory<LibraryDbContext> _dbFactory;

  public GenericRepository(IDbContextFactory<LibraryDbContext> dbFactory)
  {
    _dbFactory = dbFactory;
  }

  public async Task<IEnumerable<T>> GetAllAsync(params string[] includes)
  {
    using var context = _dbFactory.CreateDbContext();
    IQueryable<T> query = context.Set<T>();

    foreach (var include in includes)
    {
      query = query.Include(include);
    }

    return await query.ToListAsync();
  }

  public async Task<T?> GetByIdAsync(string id)
  {
    using var context = _dbFactory.CreateDbContext();
    return await context.Set<T>().FindAsync(id);
  }

  public async Task AddAsync(T entity)
  {
    using var context = _dbFactory.CreateDbContext();
    await context.Set<T>().AddAsync(entity);
    await context.SaveChangesAsync();
  }

  public async Task UpdateAsync(T entity)
  {
    using var context = _dbFactory.CreateDbContext();
    context.Set<T>().Update(entity);
    await context.SaveChangesAsync();
  }

  public async Task DeleteAsync(string id)
  {
    using var context = _dbFactory.CreateDbContext();
    var dbSet = context.Set<T>();
    var entity = await dbSet.FindAsync(id);

    if (entity != null)
    {
      dbSet.Remove(entity);
      await context.SaveChangesAsync();
    }
  }

  public async Task<IEnumerable<T>> SearchAsync(Expression<Func<T, bool>> predicate)
  {
    using var context = _dbFactory.CreateDbContext();
    return await context.Set<T>().Where(predicate).ToListAsync();
  }
}