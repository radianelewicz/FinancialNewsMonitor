namespace FinancialNewsMonitor.Responses;

public sealed record SymbolResponse(
    string Symbol,
    string Name,
    string Type,
    string Region,
    string MarketOpen,
    string MarketClose,
    string Timezone,
    string Currency,
    decimal MatchScore);