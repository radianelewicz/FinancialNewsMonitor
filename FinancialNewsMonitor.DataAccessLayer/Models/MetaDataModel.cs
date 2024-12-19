namespace FinancialNewsMonitor.DataAccessLayer.Models;

public sealed record MetaDataModel(
    string Symbol,
    string Information,
    DateOnly LastRefreshed,
    string TimeZone);
