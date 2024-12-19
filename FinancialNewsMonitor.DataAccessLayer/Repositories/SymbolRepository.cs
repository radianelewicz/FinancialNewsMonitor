using Dapper;
using FinancialNewsMonitor.DataAccessLayer.Models;
using FinancialNewsMonitor.DataAccessLayer.Repositories.Abstracts;
using FinancialNewsMonitor.DataAccessLayer.UnitOfWork;
using FinancialNewsMonitor.DataAccessLayer.Upsert;
using Microsoft.Data.SqlClient;

namespace FinancialNewsMonitor.DataAccessLayer.Repositories;

public class SymbolRepository : RepositoryBase, IRepository<SymbolModel>
{
    public SymbolRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    { }

    public async Task<SymbolModel?> GetAsync(string symbol, CancellationToken cancellationToken)
        => await unitOfWork.Connection.QuerySingleOrDefaultAsync<SymbolModel>(
            new CommandDefinition(
                "SELECT * FROM [financial].[Symbol] WHERE [Symbol] = @symbol",
                new { symbol },
                unitOfWork.Transaction,
                cancellationToken: cancellationToken));

    public async Task<IEnumerable<SymbolModel>> GetManyAsync(string? symbol, CancellationToken cancellationToken)
        => await unitOfWork.Connection.QueryAsync<SymbolModel>(
            new CommandDefinition(
                "SELECT * FROM [financial].[Symbol]",
                unitOfWork.Transaction,
                cancellationToken: cancellationToken));

    public async Task UpdateAsync(SymbolModel model, CancellationToken cancellationToken)
        => await unitOfWork.Connection.ExecuteAsync(
            new CommandDefinition(
                @"UPDATE [financial].[Symbol]
                SET [Name] = @Name, [Type] = @Type, [Region] = @Region, [MarketOpen] = @MarketOpen, [MarketClose] = @MarketClose,
                    [Timezone] = @Timezone, [Currency] = @Currency, [MatchScore] = @MatchScore
                WHERE [Symbol] = @Symbol",
                model,
                unitOfWork.Transaction,
                cancellationToken: cancellationToken));

    public async Task UpsertAsync(IEnumerable<SymbolModel> collection, CancellationToken cancellationToken)
    {
        using var dataTable = DataTableCreator.Create(collection);
        var sqlConnection = (SqlConnection)unitOfWork.Connection;
        var sqlTransaction = (SqlTransaction)unitOfWork.Transaction;
        var tempTableName = TempTableCreator.GetTempTableName();
        await TempTableCreator.CreateAsync(dataTable, sqlConnection, sqlTransaction, tempTableName);
        using var copy = new SqlBulkCopy(sqlConnection, SqlBulkCopyOptions.Default, sqlTransaction);
        copy.DestinationTableName = tempTableName;
        await copy.WriteToServerAsync(dataTable);
        var sql = $@"
MERGE INTO [financial].[Symbol] s
USING (SELECT * FROM {tempTableName}) AS src
ON s.[Symbol] = src.[Symbol]
WHEN MATCHED THEN
    UPDATE SET
        s.[Name] = src.[Name],
        s.[Type] = src.[Type],
        s.[Region] = src.[Region],
        s.[MarketOpen] = src.[MarketOpen],
        s.[MarketClose] = src.[MarketClose],
        s.[Timezone] = src.[Timezone],
        s.[Currency] = src.[Currency],
        s.[MatchScore] = src.[MatchScore]
WHEN NOT MATCHED THEN
    INSERT ([Symbol], [Name], [Type], [Region], [MarketOpen], [MarketClose], [Timezone], [Currency], [MatchScore])
    VALUES (src.[Symbol], src.[Name], src.[Type], src.[Region], src.[MarketOpen], src.[MarketClose], src.[Timezone], src.[Currency], src.[MatchScore]);";
        await sqlConnection.ExecuteAsync(sql, transaction: sqlTransaction);
    }

    public async Task DeleteAsync(string symbol, CancellationToken cancellationToken)
        => await unitOfWork.Connection.ExecuteAsync(
            new CommandDefinition(
                @"DELETE FROM [financial].[Symbol] WHERE [Symbol] = @symbol",
                new { symbol },
                unitOfWork.Transaction,
                cancellationToken: cancellationToken));

    public async Task CreateAsync(SymbolModel model, CancellationToken cancellationToken)
        => await unitOfWork.Connection.ExecuteAsync(
            new CommandDefinition(
                @"INSERT INTO [financial].[Symbol] ([Symbol], [Name], [Type], [Region], [MarketOpen], [MarketClose], [Timezone], [Currency], [MatchScore])
                VALUES (@Symbol, @Name, @Type, @Region, @MarketOpen, @MarketClose, @Timezone, @Currency, @MatchScore)",
                model,
                unitOfWork.Transaction,
                cancellationToken: cancellationToken));

    public async Task CreateManyAsync(IEnumerable<SymbolModel> collection, CancellationToken cancellationToken)
    {
        var sqlConnection = (SqlConnection)unitOfWork.Connection;
        var sqlTransaction = (SqlTransaction)unitOfWork.Transaction;
        using var bulkCopy = new SqlBulkCopy(sqlConnection, SqlBulkCopyOptions.Default, sqlTransaction);
        bulkCopy.DestinationTableName = "[financial].[Symbol]";
        await bulkCopy.WriteToServerAsync(DataTableCreator.Create(collection), cancellationToken);
    }
}
