using MongoDB.Driver;

public class MongoDbSession : IDbSession
{
    private readonly IMongoClient _client;
    private readonly IMongoDatabase _database;
    private IClientSessionHandle? _session;

    public MongoDbSession(IMongoClient client, string databaseName)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _database = _client.GetDatabase(databaseName);
    }

    public IMongoDatabase Database => _database;
    public IClientSessionHandle? Session => _session;

    public async Task BeginAsync()
    {
        if (_session != null) return;
        _session = await _client.StartSessionAsync();
        _session.StartTransaction();
    }

    public async Task CommitAsync()
    {
        try
        {
            if (_session != null && _session.IsInTransaction)
            {
                await _session.CommitTransactionAsync();
            }
        }
        catch
        {
            await RollbackAsync();
            throw;
        }
        finally
        {
            DisposeSession();
        }
    }

    public async Task RollbackAsync()
    {
        try
        {
            if (_session != null && _session.IsInTransaction)
            {
                await _session.AbortTransactionAsync();
            }
        }
        finally
        {
            DisposeSession();
        }
    }

    public Task SaveChangesAsync()
    {
        return Task.CompletedTask;
    }

    private void DisposeSession()
    {
        _session?.Dispose();
        _session = null;
    }

    public void Dispose() => DisposeSession();
    public async ValueTask DisposeAsync() { DisposeSession(); await Task.CompletedTask; }

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