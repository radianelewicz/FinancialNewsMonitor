using Dapper;
using FinancialNewsMonitor.DataAccessLayer.Models;
using FinancialNewsMonitor.DataAccessLayer.Repositories.Abstracts;
using FinancialNewsMonitor.DataAccessLayer.UnitOfWork;
using FinancialNewsMonitor.DataAccessLayer.Upsert;
using Microsoft.Data.SqlClient;

namespace FinancialNewsMonitor.DataAccessLayer.Repositories;

public class MetaDataRepository : RepositoryBase, IRepository<MetaDataModel>
{
    public MetaDataRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    { }

    public async Task<MetaDataModel?> GetAsync(string symbol, CancellationToken cancellationToken)
        => await unitOfWork.Connection.QuerySingleOrDefaultAsync<MetaDataModel>(
            new CommandDefinition(
                "SELECT * FROM [financial].[MetaData] WHERE [Symbol] = @symbol",
                symbol,
                unitOfWork.Transaction,
                cancellationToken: cancellationToken));

    public async Task<IEnumerable<MetaDataModel>> GetManyAsync(string? symbol, CancellationToken cancellationToken)
        => await unitOfWork.Connection.QueryAsync<MetaDataModel>(
            new CommandDefinition(
                string.IsNullOrEmpty(symbol)
                    ? "SELECT * FROM [financial].[MetaData]"
                    : "SELECT * FROM [financial].[MetaData] WHERE [Symbol] = @symbol",
                symbol,
                unitOfWork.Transaction,
                cancellationToken: cancellationToken));

    public async Task UpdateAsync(MetaDataModel model, CancellationToken cancellationToken)
        => await unitOfWork.Connection.ExecuteAsync(
            new CommandDefinition(
                @"UPDATE [financial].[MetaData]
                SET [Information] = @Information, [LastRefreshed] = @LastRefreshed, [TimeZone] = @TimeZone
                WHERE [Symbol] = @Symbol",
                model,
                unitOfWork.Transaction,
                cancellationToken: cancellationToken));

    public async Task UpsertAsync(IEnumerable<MetaDataModel> collection, CancellationToken cancellationToken)
    {
        using var dataTable = DataTableCreator.Create(collection);
        var sqlConnection = (SqlConnection)unitOfWork.Connection;
        var sqlTransaction = (SqlTransaction)unitOfWork.Transaction;
        var tempTableName = TempTableCreator.GetTempTableName();
        await TempTableCreator.CreateAsync(dataTable, sqlConnection, sqlTransaction, tempTableName);
        using var copy = new SqlBulkCopy(sqlConnection, SqlBulkCopyOptions.Default, sqlTransaction);
        await copy.WriteToServerAsync(dataTable);
        var sql = $@"
MERGE INTO [financial].[MetaData] md
USING (SELECT * {tempTableName}) AS src
ON md.[Symbol] = src.[Symbol]
WHEN MATCHED THEN
    UPDATE SET
        md.[Information] = src.[Information],
        md.[LastRefreshed] = src.[LastRefreshed],
        md.[TimeZone] = src.[TimeZone]
WHEN NOT MATCHED THEN
    INSERT ([Information], [LastRefreshed], [TimeZone])
    VALUES (src.[Information], src.[LastRefreshed], src.[TimeZone])";
        await sqlConnection.ExecuteAsync(sql, transaction: sqlTransaction);
    }

    public async Task DeleteAsync(string symbol, CancellationToken cancellationToken)
        => await unitOfWork.Connection.ExecuteAsync(
            new CommandDefinition(
                "DELETE FROM [financial].[MetaData] WHERE [Symbol] = @symbol",
                symbol,
                unitOfWork.Transaction,
                cancellationToken: cancellationToken));

    public async Task CreateAsync(MetaDataModel model, CancellationToken cancellationToken)
        => await unitOfWork.Connection.ExecuteAsync(
            new CommandDefinition(
                @"INSERT INTO [financia].[MetaData] ([Information], [Symbol], [LastRefreshed], [TimeZone])
                VALUES (@Information, @Symbol, @LastRefreshed, @TimeZone)",
                model,
                unitOfWork.Transaction,
                cancellationToken: cancellationToken));

    public async Task CreateManyAsync(IEnumerable<MetaDataModel> collection, CancellationToken cancellationToken)
    {
        var sqlConnection = (SqlConnection)unitOfWork.Connection;
        var sqlTransaction = (SqlTransaction)unitOfWork.Transaction;
        using var bulkCopy = new SqlBulkCopy(sqlConnection, SqlBulkCopyOptions.Default, sqlTransaction);
        bulkCopy.DestinationTableName = "[financial].[MetaData]";
        await bulkCopy.WriteToServerAsync(DataTableCreator.Create(collection), cancellationToken);
    }

}
