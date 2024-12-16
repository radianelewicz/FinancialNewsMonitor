using FinancialNewsMonitor.DataAccessLayer.UnitOfWork;
using Microsoft.Extensions.Configuration;

namespace FinancialNewsMonitor.DataAccessLayer.Resolvers;

public interface IUnitOfWorkSessionResolver
{
    UnitofWorkSession Resolve(string? connectionStringName = null);
}

public class UnitOfWorkSessionResolver : IUnitOfWorkSessionResolver
{
    private const string _defaultConnectionStringName = "FinancialConnectionString";
    private readonly IConfiguration _configuration;

    public UnitOfWorkSessionResolver(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public UnitofWorkSession Resolve(string? connectionStringName = null)
    {
        var connectionString = string.IsNullOrWhiteSpace(connectionStringName)
            ? _configuration.GetConnectionString(_defaultConnectionStringName)
            : _configuration.GetConnectionString(connectionStringName);

        if (connectionString == null)
        {
            throw new ArgumentNullException(nameof(connectionString));
        }

        return new UnitofWorkSession(connectionString);
    }
}