using System;
using System.Threading.Tasks;

public interface IDbSession : IDisposable, IAsyncDisposable
{
    Task BeginAsync();
    Task CommitAsync();
    Task RollbackAsync();
    Task SaveChangesAsync();

    Task ExecuteInAsync(Func<Task> action);
}
