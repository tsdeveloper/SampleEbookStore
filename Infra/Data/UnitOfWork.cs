using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DTOs;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Entity.Validation;
using Infra.Repositories;

namespace Infra.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly SampleEbookStoreContext _context;
    private IDbContextTransaction? _currentTransaction;
    private string _errorMessage = string.Empty;
    private Hashtable _repositories;

    public UnitOfWork(SampleEbookStoreContext context)
    {
        _context = context;
    }

    public async Task BeginTransactionAsync()
    {
        if (_currentTransaction is not null)
            throw new InvalidOperationException("A transaction has already been started.");
        _currentTransaction = _context.Database.BeginTransaction();
    }

    public async Task CommitAsync()
    {
        if (_currentTransaction is null)
            throw new InvalidOperationException("A transaction has not been started.");
        try
        {
            //Commits the underlying store transaction
            await _currentTransaction.CommitAsync();
            _currentTransaction.Dispose();
            _currentTransaction = null;
        }
        catch (DbUpdateException ex2)
        {
            if (_currentTransaction is not null)
                await _currentTransaction.RollbackAsync();

            _currentTransaction = null;
            throw ex2;
        }
        catch (Exception ex)
        {
            if (_currentTransaction is not null)
                await _currentTransaction.RollbackAsync();

            _currentTransaction = null;
            throw ex;
        }
    }

    public async Task<GenericResponse<int>> SaveChangesAsync()
    {
        var result = new GenericResponse<int>();

        try
        {
            //Calling DbContext Class SaveChanges method 
            result.Data = await _context.SaveChangesAsync();
        }
        catch (DbEntityValidationException dbEx)
        {
            foreach (var validationErrors in dbEx.EntityValidationErrors)
            foreach (var validationError in validationErrors.ValidationErrors)
                _errorMessage = _errorMessage +
                                $"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage} {Environment.NewLine}";
            result.Error = new MessageResponse();
            result.Error.Message = _errorMessage;
            result.Error.Status = 500;
        }
        catch (Exception ex)
        {
            result.Error = new MessageResponse();
            result.Error.Message = "Error ao salvar os dados!";
            result.Error.Status = 500;
        }

        return result;
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
    {
  

        if (_repositories == null) _repositories = new Hashtable();
        
      _context.ChangeTracker.LazyLoadingEnabled= false;

        var type = typeof(TEntity).Name;

        if (!_repositories.ContainsKey(type))
        {
            var repositoryType = typeof(GenericRepository<>);
            var repositoryInstance =
                Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);

            _repositories.Add(type, repositoryInstance);
        }

        return (IGenericRepository<TEntity>)_repositories[type];
    }

    public async Task RollbackAsync()
    {
        //Rolls back the underlying store transaction
        await _currentTransaction.RollbackAsync();
        //The Dispose Method will clean up this transaction object and ensures Entity Framework
        //is no longer using that transaction.
        _currentTransaction.Dispose();
    }
}