using FinancialNewsMonitor.DataAccessLayer.Models;
using FinancialNewsMonitor.DataAccessLayer.Resolvers;

namespace FinancialNewsMonitor.Services;

public interface IFinancialService
{
    Task<IEnumerable<SymbolModel>> GetSymbolsAsync(CancellationToken cancellationToken);
    Task<StockDataModel> GeStockDataAsync(string symbol, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(string symbol, CancellationToken cancellationToken);
    Task<bool> CreateSymbolStockDataAsync(SymbolStockDataModel symbolStockDataModel, CancellationToken cancellationToken);
    Task<bool> UpdateSymbolStockDataAsync(SymbolStockDataModel symbolStockDataModel, CancellationToken cancellationToken);
}

public class FinancialService : IFinancialService
{
    private readonly IUnitOfWorkSessionResolver _unitOfWorkSessionResolver;

    public FinancialService(
        IUnitOfWorkSessionResolver unitOfWorkSessionResolver)
    {
        _unitOfWorkSessionResolver = unitOfWorkSessionResolver;
    }

    public async Task<IEnumerable<SymbolModel>> GetSymbolsAsync(CancellationToken cancellationToken)
    {
        using var session = _unitOfWorkSessionResolver.Resolve();
        return await session.UnitOfWork.SymbolRepository.GetManyAsync(null, cancellationToken);
    }

    public async Task<StockDataModel> GeStockDataAsync(string symbol, CancellationToken cancellationToken)
    {
        using var session = _unitOfWorkSessionResolver.Resolve();

        var metaDataModel = await session.UnitOfWork.MetaDataRepository.GetAsync(symbol, cancellationToken);
        if (metaDataModel == null)
        {
            throw new ArgumentNullException(nameof(metaDataModel));
        }

        var stockValuesModel = await session.UnitOfWork.StockValueRepository.GetManyAsync(symbol, cancellationToken);
        if (stockValuesModel == null)
        {
            throw new ArgumentNullException(nameof(stockValuesModel));
        }

        return new StockDataModel(metaDataModel!, stockValuesModel);
    }

    public async Task<bool> DeleteAsync(string symbol, CancellationToken cancellationToken)
    {
        using var session = _unitOfWorkSessionResolver.Resolve();
        if (await session.UnitOfWork.SymbolRepository.GetAsync(symbol, cancellationToken) == null)
        {
            return false;
        }

        try
        {
            session.UnitOfWork.Begin();

            await session.UnitOfWork.StockValueRepository.DeleteAsync(symbol, cancellationToken);
            await session.UnitOfWork.MetaDataRepository.DeleteAsync(symbol, cancellationToken);
            await session.UnitOfWork.SymbolRepository.DeleteAsync(symbol, cancellationToken);

            session.UnitOfWork.Commit();
        }
        catch(Exception)
        {
            session.UnitOfWork.Rollback();

            throw;
        }

        return true;
    }

    public async Task<bool> CreateSymbolStockDataAsync(SymbolStockDataModel symbolStockDataModel, CancellationToken cancellationToken)
    {
        using var session = _unitOfWorkSessionResolver.Resolve();
        if (await session.UnitOfWork.SymbolRepository.GetAsync(symbolStockDataModel.Symbol.Symbol, cancellationToken) != null)
        {
            return false;
        }

        try
        {
            session.UnitOfWork.Begin();

            await session.UnitOfWork.SymbolRepository.CreateAsync(symbolStockDataModel.Symbol, cancellationToken);
            await session.UnitOfWork.MetaDataRepository.CreateAsync(symbolStockDataModel.StockData.MetaData, cancellationToken);
            await session.UnitOfWork.StockValueRepository.CreateManyAsync(symbolStockDataModel.StockData.StockValues, cancellationToken);

            session.UnitOfWork.Commit();
        }
        catch (Exception)
        {
            session.UnitOfWork.Rollback();

            throw;
        }

        return true;
    }

    public async Task<bool> UpdateSymbolStockDataAsync(SymbolStockDataModel symbolStockDataModel, CancellationToken cancellationToken)
    {
        using var session = _unitOfWorkSessionResolver.Resolve();
        if (await session.UnitOfWork.SymbolRepository.GetAsync(symbolStockDataModel.Symbol.Symbol, cancellationToken) == null)
        {
            return false;
        }

        try
        {
            session.UnitOfWork.Begin();

            await session.UnitOfWork.SymbolRepository.UpdateAsync(symbolStockDataModel.Symbol, cancellationToken);
            await session.UnitOfWork.MetaDataRepository.UpdateAsync(symbolStockDataModel.StockData.MetaData, cancellationToken);
            await session.UnitOfWork.StockValueRepository.UpsertAsync(symbolStockDataModel.StockData.StockValues, cancellationToken);

            session.UnitOfWork.Commit();
        }
        catch(Exception)
        {
            session.UnitOfWork.Rollback();

            throw;
        }

        return true;
    }
}
