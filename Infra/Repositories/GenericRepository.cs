using Core.Interfaces;
using Infra.Data;
using Infra.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
  private readonly SampleEbookStoreContext _context;
  public GenericRepository(SampleEbookStoreContext context)
  {
    _context = context;

  }
  public void Add(TEntity entity)
  {
    _context.Set<TEntity>().Add(entity);
  }

  public async Task<int> CountAsync(ISpecification<TEntity> spec)
  {
    return await ApplySpecification(spec).CountAsync();
  }

  public void Delete(TEntity entity)
  {
    _context.Set<TEntity>().Remove(entity);
  }

  public async Task<TEntity> GetByIdAsync(ISpecification<TEntity> spec)
  {
    return await ApplySpecification(spec).FirstOrDefaultAsync();
  }

  public async Task<TEntity> GetEntityWithSpec(ISpecification<TEntity> spec)
  {
    return await ApplySpecification(spec).FirstOrDefaultAsync();
  }

  public async Task<bool> GetExistEntityWithSpec(ISpecification<TEntity> spec)
  {
    return await ApplySpecification(spec).AsNoTracking().FirstOrDefaultAsync() != null;
  }

  public async Task<IReadOnlyList<TEntity>> ListAllAsync()
  {
    return await ApplySpecification(default).ToListAsync();
  }

  public async Task<IList<TEntity>> ListAllAsync(ISpecification<TEntity> spec)
  {
    return await ApplySpecification(spec).ToListAsync();
  }

  public async Task<IReadOnlyList<TEntity>> ListReadOnlyListAsync(ISpecification<TEntity> spec)
  {
    return await ApplySpecification(spec).ToListAsync();
  }

  public void Update(TEntity entity)
  {
    _context.ChangeTracker.Clear();
    _context.Entry(entity).State = EntityState.Modified;
  }

  private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> spec)
  {
    return SpecificationEvaluator<TEntity>.GetQuery(_context.Set<TEntity>().AsQueryable(), spec);
  }
}
