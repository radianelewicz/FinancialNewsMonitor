using Dapper;
using FinancialNewsMonitor.DataAccessLayer.Models;
using FinancialNewsMonitor.DataAccessLayer.Repositories.Abstracts;
using FinancialNewsMonitor.DataAccessLayer.UnitOfWork;
using FinancialNewsMonitor.DataAccessLayer.Upsert;
using Microsoft.Data.SqlClient;

namespace FinancialNewsMonitor.DataAccessLayer.Repositories;

public class StockValueRepository : RepositoryBase, IRepository<StockValueModel>
{
    public StockValueRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    { }

    public async Task<StockValueModel?> GetAsync(string symbol, CancellationToken cancellationToken)
        => await unitOfWork.Connection.QueryFirstOrDefaultAsync<StockValueModel>(
            new CommandDefinition(
                "SELECT * FROM [financial].[StockValue] WHERE [Symbol] = @symbol",
                symbol,
                unitOfWork.Transaction,
                cancellationToken: cancellationToken));

    public async Task<IEnumerable<StockValueModel>> GetManyAsync(string? symbol, CancellationToken cancellationToken)
        => await unitOfWork.Connection.QueryAsync<StockValueModel>(
            new CommandDefinition(
                string.IsNullOrEmpty(symbol)
                    ? "SELECT * FROM [financial].[StockValue]"
                    : "SELECT * FROM [financial].[StockValue] WHERE [Symbol] = @symbol",
                symbol,
                unitOfWork.Transaction,
                cancellationToken: cancellationToken));

    public async Task UpdateAsync(StockValueModel model, CancellationToken cancellationToken)
        => await unitOfWork.Connection.ExecuteAsync(
            new CommandDefinition(
                @"UPDATE [financial].[MetaData]
                SET [Information] = @Information, [LastRefreshed] = @LastRefreshed, [TimeZone] = @TimeZone
                WHERE [Symbol] = @Symbol",
                model,
                unitOfWork.Transaction,
                cancellationToken: cancellationToken));

    public async Task UpsertAsync(IEnumerable<StockValueModel> collection, CancellationToken cancellationToken)
    {
        using var dataTable = DataTableCreator.Create(collection);
        var sqlConnection = (SqlConnection)unitOfWork.Connection;
        var sqlTransaction = (SqlTransaction)unitOfWork.Transaction;
        var tempTableName = TempTableCreator.GetTempTableName();
        await TempTableCreator.CreateAsync(dataTable, sqlConnection, sqlTransaction, tempTableName);
        using var copy = new SqlBulkCopy(sqlConnection, SqlBulkCopyOptions.Default, sqlTransaction);
        await copy.WriteToServerAsync(dataTable);
        var sql = $@"
MERGE INTO [financial].[StockValue] sv
USING (SELECT * {tempTableName}) AS src
ON sv.[Symbol] = src.[Symbol] AND sv.[Date] = src.[Date] 
WHEN MATCHED THEN
    UPDATE SET
        s.[Open] = src.[Open],
        s.[High] = src.[High],
        s.[Low] = src.[Low],
        s.[Close] = src.[Close],
        s.[Volume] = src.[Volume]
WHEN NOT MATCHED THEN
    INSERT ([Symbol], [Date], [Open], [High], [Low], [Close], [Volume])
    VALUES (src.[Symbol], src.[Date], src.[Open], src.[High], src.[Low], src.[Close], src.[Volume])";
        await sqlConnection.ExecuteAsync(sql, transaction: sqlTransaction);
    }

    public async Task DeleteAsync(string symbol, CancellationToken cancellationToken)
        => await unitOfWork.Connection.ExecuteAsync(
            new CommandDefinition(
                "DELETE FROM [financial].[StockValue] WHERE [Symbol] = @symbol",
                symbol,
                unitOfWork.Transaction,
                cancellationToken: cancellationToken));

    public async Task CreateAsync(StockValueModel model, CancellationToken cancellationToken)
        => await unitOfWork.Connection.ExecuteAsync(
            new CommandDefinition(
                @"INSERT INTO [financia].[StockValue] ([Symbol], [Date], [Open], [High], [Low], [Close], [Volume])
                VALUES (@Symbol, @Date, @Open, @High, @Low, @Close, @Volume)",
                model,
                unitOfWork.Transaction,
                cancellationToken: cancellationToken));

    public async Task CreateManyAsync(IEnumerable<StockValueModel> collection, CancellationToken cancellationToken)
    {
        var sqlConnection = (SqlConnection)unitOfWork.Connection;
        var sqlTransaction = (SqlTransaction)unitOfWork.Transaction;
        using var bulkCopy = new SqlBulkCopy(sqlConnection, SqlBulkCopyOptions.Default, sqlTransaction);
        bulkCopy.DestinationTableName = "[financial].[StockValue]";
        await bulkCopy.WriteToServerAsync(DataTableCreator.Create(collection), cancellationToken);
    }
}
