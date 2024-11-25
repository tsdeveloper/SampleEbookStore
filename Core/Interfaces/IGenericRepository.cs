using Core.Specification;

namespace Core.Interfaces;

public interface IGenericRepository<TEntity> where TEntity : class
{
  Task<TEntity> GetByIdAsync(ISpecification<TEntity> spec);
  Task<IReadOnlyList<TEntity>> ListAllAsync();
  Task<TEntity> GetEntityWithSpec(ISpecification<TEntity> spec);
  Task<bool> GetExistEntityWithSpec(ISpecification<TEntity> spec);
  Task<IReadOnlyList<TEntity>> ListReadOnlyListAsync(ISpecification<TEntity> spec);
  Task<IList<TEntity>> ListAllAsync(ISpecification<TEntity> spec);
  Task<int> CountAsync(ISpecification<TEntity> spec);
  void Add(TEntity entity);
  void Update(TEntity entity);
  void Delete(TEntity entity);

}
