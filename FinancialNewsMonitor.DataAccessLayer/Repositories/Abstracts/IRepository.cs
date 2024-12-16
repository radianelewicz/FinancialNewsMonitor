namespace FinancialNewsMonitor.DataAccessLayer.Repositories.Abstracts;

public interface IRepository<T>
{
    Task<T?> GetAsync(string symbol, CancellationToken cancellationToken);
    Task<IEnumerable<T>> GetManyAsync(string? symbol, CancellationToken cancellationToken);
    Task UpdateAsync(T model, CancellationToken cancellationToken);
    Task UpsertAsync(IEnumerable<T> collection, CancellationToken cancellationToken);
    Task DeleteAsync(string symbol, CancellationToken cancellationToken);
    Task CreateAsync(T model, CancellationToken cancellationToken);
    Task CreateManyAsync(IEnumerable<T> collection, CancellationToken cancellationToken);
}
