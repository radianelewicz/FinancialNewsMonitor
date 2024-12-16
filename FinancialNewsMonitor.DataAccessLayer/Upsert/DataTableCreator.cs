using System.Data;
using System.Reflection;

namespace FinancialNewsMonitor.DataAccessLayer.Upsert;

public static class DataTableCreator
{
    public static DataTable Create<T>(IEnumerable<T> items)
        where T : class
    {
        var dataTable = new DataTable();
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            var dataColumn = new DataColumn();
            dataColumn.ColumnName = property.Name;

            var nullabeType = Nullable.GetUnderlyingType(property.PropertyType);
            if (nullabeType == null)
            {
                dataColumn.DataType = property.PropertyType;
            }
            else
            {
                dataColumn.DataType = nullabeType;
                dataColumn.AllowDBNull = true;
            }

            dataTable.Columns.Add(dataColumn);
        }

        foreach (var item in items)
        {
            var values = new object[properties.Length];

            for (int i = 0; i < properties.Length; i++)
            {
                values[i] = properties[i].GetValue(item, null)!;
            }

            dataTable.Rows.Add(values);
        }


        return dataTable;
    }
}
