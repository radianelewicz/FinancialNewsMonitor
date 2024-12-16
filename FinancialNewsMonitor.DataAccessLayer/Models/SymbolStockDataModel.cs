namespace FinancialNewsMonitor.DataAccessLayer.Models;

public sealed record SymbolStockDataModel(
    SymbolModel Symbol,
    StockDataModel StockData);
