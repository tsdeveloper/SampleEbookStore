using Core.DTOs;

namespace Core.Interfaces;

public interface IUnitOfWork
{
  IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;
  Task BeginTransactionAsync();

  Task<GenericResponse<int>> SaveChangesAsync();

  Task CommitAsync();

  Task RollbackAsync();
}
