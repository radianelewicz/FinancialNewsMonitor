namespace FinancialNewsMonitor.Requests;

public sealed record StockDataRequest(
    MetaDataRequest MetaDataRequest,
    IReadOnlyDictionary<DateOnly, StockValueRequest> StockValuesRequest);

public sealed record MetaDataRequest(
    string Information,
    string Symbol,
    DateOnly LastRefreshed,
    string TimeZone);

public sealed record StockValueRequest(
    decimal Open,
    decimal High,
    decimal Low,
    decimal Close,
    int Volume);
