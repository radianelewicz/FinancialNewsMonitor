using System.Text.Json.Serialization;

namespace FinancialNewsMonitor.Services.ExternalApiModels;

public sealed record StockDataApiResult(
    [property: JsonPropertyName("Meta Data")] MetaDataApiResult MetaDataResponse,
    [property: JsonPropertyName("Time Series (Daily)")] IReadOnlyDictionary<string, StockValueApiResult> StockValues);

public sealed record MetaDataApiResult(
    [property: JsonPropertyName("1. Information")] string Information,
    [property: JsonPropertyName("2. Symbol")] string Symbol,
    [property: JsonPropertyName("3. Last Refreshed")] string LastRefreshed,
    [property: JsonPropertyName("5. Time Zone")] string TimeZone);

public sealed record StockValueApiResult(
    [property: JsonPropertyName("1. open")] string Open,
    [property: JsonPropertyName("2. high")] string High,
    [property: JsonPropertyName("3. low")] string Low,
    [property: JsonPropertyName("4. close")] string Close,
    [property: JsonPropertyName("5. volume")] string Volume);
