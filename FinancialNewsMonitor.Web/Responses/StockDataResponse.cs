namespace FinancialNewsMonitor.Responses;

public sealed record StockDataResponse(
    MetaDataResponse MetaDataResponse,
    IEnumerable<StockValueResponse> StockValuesResponse);

public sealed record MetaDataResponse(
    string Information,
    DateOnly LastRefreshed,
    string TimeZone);

public sealed record StockValueResponse(
    DateOnly Date,
    decimal Open,
    decimal High,
    decimal Low,
    decimal Close,
    int Volume);
