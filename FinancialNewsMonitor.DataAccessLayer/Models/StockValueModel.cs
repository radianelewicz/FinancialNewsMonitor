namespace FinancialNewsMonitor.DataAccessLayer.Models;

public sealed record StockValueModel(
    string Symbol,
    DateOnly Date,
    decimal Open,
    decimal High,
    decimal Low,
    decimal Close,
    int Volume);
