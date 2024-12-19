namespace FinancialNewsMonitor.Requests;

public sealed record StockDataRequest(
    MetaDataRequest MetaDataRequest,
    IEnumerable<StockValueRequest> StockValuesRequest);

public sealed record MetaDataRequest(
    string Information,
    DateOnly LastRefreshed,
    string TimeZone);

public sealed record StockValueRequest(
    DateOnly Date,
    decimal Open,
    decimal High,
    decimal Low,
    decimal Close,
    int Volume);
