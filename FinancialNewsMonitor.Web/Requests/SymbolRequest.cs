namespace FinancialNewsMonitor.Requests;

public sealed record SymbolRequest(
    string Symbol,
    string Name,
    string Type,
    string Region,
    string MarketOpen,
    string MarketClose,
    string Timezone,
    string Currency,
    decimal MatchScore);
