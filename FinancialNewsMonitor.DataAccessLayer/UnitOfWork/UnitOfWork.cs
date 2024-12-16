using FinancialNewsMonitor.DataAccessLayer.Models;
using FinancialNewsMonitor.DataAccessLayer.Repositories;
using FinancialNewsMonitor.DataAccessLayer.Repositories.Abstracts;
using FinancialNewsMonitor.DataAccessLayer.Repositories.Factories;
using System.Data;

namespace FinancialNewsMonitor.DataAccessLayer.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    Guid Id { get; }
    IDbConnection Connection { get; }
    IDbTransaction Transaction { get; }
    void Begin();
    void Commit();
    void Rollback();
}

public sealed class UnitOfWork : IUnitOfWork
{
    #region Repositories
    private IRepository<SymbolModel> _symbolRepository = null!;
    public IRepository<SymbolModel> SymbolRepository
    {
        get
        {

            if (_symbolRepository == null)
            {
                _symbolRepository = RepositoryFactory.CreateRepository<SymbolModel, SymbolRepository>(this);
            }
            return _symbolRepository;
        }
    }

    private IRepository<MetaDataModel> _metaDataRepository = null!;
    public IRepository<MetaDataModel> MetaDataRepository
    {
        get
        {

            if (_metaDataRepository == null)
            {
                _metaDataRepository = RepositoryFactory.CreateRepository<MetaDataModel, MetaDataRepository>(this);
            }
            return _metaDataRepository;
        }
    }

    private IRepository<StockValueModel> _stockValueRepository = null!;
    public IRepository<StockValueModel> StockValueRepository
    {
        get
        {

            if (_stockValueRepository == null)
            {
                _stockValueRepository = RepositoryFactory.CreateRepository<StockValueModel, StockValueRepository>(this);
            }
            return _stockValueRepository;
        }
    }
    #endregion Repositories

    Guid _id = Guid.Empty;
    Guid IUnitOfWork.Id
    {
        get { return _id; }
    }

    IDbConnection _connection = null!;
    IDbConnection IUnitOfWork.Connection
    {
        get { return _connection; }
    }

    IDbTransaction _transaction = null!;
    IDbTransaction IUnitOfWork.Transaction
    {
        get { return _transaction; }
    }

    internal UnitOfWork(IDbConnection connection)
    {
        _id = Guid.NewGuid();
        _connection = connection;
    }

    public void Begin()
    {
        _transaction = _connection.BeginTransaction();
    }

    public void Commit()
    {
        _transaction.Commit();
        Dispose();
    }

    public void Rollback()
    {
        _transaction.Rollback();
        Dispose();
    }

    public void Dispose()
    {
        if (_transaction != null)
        {
            _transaction.Dispose();
        }
            
        _transaction = null!;
    }
}
