using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FinancialNewsMonitor.DataAccessLayer.Upsert;

public static class TempTableCreator
{
    private const string _createTempTableSql = "CREATE TABLE {0} {1};";

    public static string GetTempTableName()
        => string.Concat("#TempTable", Guid.NewGuid().ToString().Replace("-", ""));

    public static async Task CreateAsync(
        DataTable dataTable,
        SqlConnection sqlConnection,
        SqlTransaction sqlTransaction,
        string tempTableName)
    {
        var result = await sqlConnection.ExecuteAsync(GetCreateTempTableSqlQuery(
            tempTableName,
            dataTable.Columns),
            transaction: sqlTransaction);

        if (result == 0)
        {
            throw new InvalidOperationException();
        }
    }

    private static string GetCreateTempTableSqlQuery(string tableName, DataColumnCollection dataColumnCollection)
        => string.Concat(
            _createTempTableSql,
            tableName,
            string.Join(",", dataColumnCollection.Cast<DataColumn>().ToList().Select(column => $"{column.ColumnName} {GetSqlType(column)}")));

    private static string GetSqlType(DataColumn dataColumn)
    {
        string? type;

        if (dataColumn.DataType == typeof(string))
        {
            type = "NVARCHAR(255) COLLATE database_default";
        }
        else if (dataColumn.DataType == typeof(int))
        {
            type = "INT";
        }
        else if (dataColumn.DataType == typeof(bool))
        {
            type = "BIT";
        }
        else if (dataColumn.DataType == typeof(short))
        {
            type = "SMALLINT";
        }
        else if (dataColumn.DataType == typeof(byte))
        {
            type = "TINYINT";
        }
        else if (dataColumn.DataType == typeof(DateOnly))
        {
            type = "DATE";
        }
        else if (dataColumn.DataType == typeof(decimal))
        {
            type = "DECIMAL(7,4)";
        }
        else
        {
            throw new InvalidOperationException($"Invalid data type: {dataColumn.DataType}");
        }

        if (!dataColumn.AllowDBNull)
        {
            type += "NOT NULL";
        }

        return type;
    }
}
