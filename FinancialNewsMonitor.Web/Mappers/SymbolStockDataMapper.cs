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
                    symbolRequest.Symbol,
                    stockDataRequest.MetaDataRequest.Information,
                    stockDataRequest.MetaDataRequest.LastRefreshed,
                    stockDataRequest.MetaDataRequest.TimeZone),
                    stockDataRequest.StockValuesRequest.Select(x => Map(x, symbolRequest.Symbol))));


    private StockValueModel Map(StockValueRequest request, string symbol)
        => new StockValueModel(
            symbol,
            request.Date,
            request.Open,
            request.High,
            request.Low,
            request.Close,
            request.Volume);
}
