namespace FinancialNewsMonitor.Requests;

public sealed record UpdateSymbolStockDataRequest(
    SymbolRequest SymbolRequest,
    StockDataRequest StockDataRequest);
