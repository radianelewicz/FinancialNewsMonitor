using System.Text.Json.Serialization;

namespace FinancialNewsMonitor.Services.ExternalApiModels;

public sealed record SymbolApiResult(
    [property: JsonPropertyName("bestMatches")] IReadOnlyCollection<SymbolResult> Symbols);

public sealed record SymbolResult(
    [property: JsonPropertyName("1. symbol")] string Symbol,
    [property: JsonPropertyName("2. name")] string Name,
    [property: JsonPropertyName("3. type")] string Type,
    [property: JsonPropertyName("4. region")] string Region,
    [property: JsonPropertyName("5. marketOpen")] string MarketOpen,
    [property: JsonPropertyName("6. marketClose")] string MarketClose,
    [property: JsonPropertyName("7. timezone")] string Timezone,
    [property: JsonPropertyName("8. currency")] string Currency,
    [property: JsonPropertyName("9. matchScore")] string MatchScore);
