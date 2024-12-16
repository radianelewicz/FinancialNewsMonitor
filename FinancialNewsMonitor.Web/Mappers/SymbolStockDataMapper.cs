using FinancialNewsMonitor.DataAccessLayer.Models;
using FinancialNewsMonitor.Requests;

namespace FinancialNewsMonitor.Mappers;

public interface ISymbolStockDataMapper
{
    SymbolStockDataModel Map(CreateSymbolStockDataRequest request);
    SymbolStockDataModel Map(UpdateSymbolStockDataRequest request);
}

public class SymbolStockDataMapper : ISymbolStockDataMapper
{
    public SymbolStockDataModel Map(CreateSymbolStockDataRequest request)
        => Map(request.SymbolRequest, request.StockDataRequest);

    public SymbolStockDataModel Map(UpdateSymbolStockDataRequest request)
        => Map(request.SymbolRequest, request.StockDataRequest);

    private SymbolStockDataModel Map(SymbolRequest symbolRequest, StockDataRequest stockDataRequest)
        => new SymbolStockDataModel(
            new SymbolModel(
                symbolRequest.Symbol,
                symbolRequest.Name,
                symbolRequest.Type,
                symbolRequest.Region,
                symbolRequest.MarketOpen,
                symbolRequest.MarketClose,
                symbolRequest.Timezone,
                symbolRequest.Currency,
                symbolRequest.MatchScore),
            new StockDataModel(
                new MetaDataModel(
                    stockDataRequest.MetaDataRequest.Information,
                    stockDataRequest.MetaDataRequest.Symbol,
                    stockDataRequest.MetaDataRequest.LastRefreshed,
                    stockDataRequest.MetaDataRequest.TimeZone),
                Map(stockDataRequest.StockValuesRequest, stockDataRequest.MetaDataRequest.Symbol)));

    private IReadOnlyCollection<StockValueModel> Map(IReadOnlyDictionary<DateOnly, StockValueRequest> stockValuesRequest, string symbol)
    {
        var result = new List<StockValueModel>();

        foreach (var kvp in stockValuesRequest)
        {
            result.Add(Map(kvp.Value, kvp.Key, symbol));
        }

        return result;
    }

    private StockValueModel Map(StockValueRequest request, DateOnly date, string symbol)
        => new StockValueModel(
            symbol,
            date,
            request.Open,
            request.High,
            request.Low,
            request.Close,
            request.Volume);
}
