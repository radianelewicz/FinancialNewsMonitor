namespace FinancialNewsMonitor.Requests;

public sealed record CreateSymbolStockDataRequest(
    SymbolRequest SymbolRequest,
    StockDataRequest StockDataRequest);