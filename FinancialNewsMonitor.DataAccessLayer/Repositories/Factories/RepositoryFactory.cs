using FinancialNewsMonitor.DataAccessLayer.Repositories.Abstracts;

namespace FinancialNewsMonitor.DataAccessLayer.Repositories.Factories;

internal class RepositoryFactory
{
    internal static TRepository CreateRepository<T, TRepository>(params object[] args)
        where TRepository : IRepository<T>
        => (TRepository)Activator.CreateInstance(typeof(TRepository), args)!
            ?? throw new ArgumentNullException(typeof(TRepository).Name);
}
