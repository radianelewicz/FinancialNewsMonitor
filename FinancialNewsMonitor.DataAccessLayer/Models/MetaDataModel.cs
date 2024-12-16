namespace FinancialNewsMonitor.DataAccessLayer.Models;

public sealed record MetaDataModel(
    string Information,
    string Symbol,
    DateOnly LastRefreshed,
    string TimeZone);
