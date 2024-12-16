using Microsoft.Data.SqlClient;
using System.Data;

namespace FinancialNewsMonitor.DataAccessLayer.UnitOfWork;

public sealed class UnitofWorkSession : IDisposable
{
    private IDbConnection _connection = null;
    private UnitOfWork _unitOfWork = null;

    public UnitOfWork UnitOfWork
    {
        get { return _unitOfWork; }
    }

    public UnitofWorkSession(string connectionString)
    {
        _connection = new SqlConnection(connectionString);
        _connection.Open();
        _unitOfWork = new UnitOfWork(_connection);
    }

    public void Dispose()
    {
        _unitOfWork.Dispose();
        _connection.Dispose();
    }
}
