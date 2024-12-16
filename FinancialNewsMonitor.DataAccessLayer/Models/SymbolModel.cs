namespace FinancialNewsMonitor.DataAccessLayer.Models;

public sealed record SymbolModel(
    string Symbol,
    string Name,
    string Type,
    string Region,
    string MarketOpen,
    string MarketClose,
    string Timezone,
    string Currency,
    decimal MatchScore);
