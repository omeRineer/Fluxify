using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;

public class CoreDbSession<TContext> : IDbSession where TContext : DbContext
{
    private readonly TContext _context;
    private IDbContextTransaction? _transaction;

    public CoreDbSession(TContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public TContext Context => _context;

    public async Task BeginAsync()
    {
        if (_transaction != null) return;
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        try
        {
            await SaveChangesAsync();
            if (_transaction != null) await _transaction.CommitAsync();
        }
        catch
        {
            await RollbackAsync();
            throw;
        }
        finally
        {
            DisposeTransaction();
        }
    }

    public async Task RollbackAsync()
    {
        try
        {
            if (_transaction != null) await _transaction.RollbackAsync();
        }
        finally
        {
            DisposeTransaction();
        }
    }

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

    private void DisposeTransaction()
    {
        _transaction?.Dispose();
        _transaction = null;
    }

    public void Dispose() { DisposeTransaction(); _context.Dispose(); }
    public async ValueTask DisposeAsync() { DisposeTransaction(); await _context.DisposeAsync(); }

    public async Task ExecuteInAsync(Func<Task> action)
    {
        await BeginAsync();

        try
        {
            await action();

            await SaveChangesAsync();

            await CommitAsync();
        }
        catch
        {
            await RollbackAsync();
            throw;
        }
    }
}
