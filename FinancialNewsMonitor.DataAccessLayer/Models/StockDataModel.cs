namespace FinancialNewsMonitor.DataAccessLayer.Models;

public sealed record StockDataModel(
    MetaDataModel MetaData,
    IEnumerable<StockValueModel> StockValues);
