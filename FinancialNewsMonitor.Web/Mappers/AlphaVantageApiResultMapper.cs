using FinancialNewsMonitor.Responses;
using FinancialNewsMonitor.Services.ExternalApiModels;
using System.Globalization;

namespace FinancialNewsMonitor.Mappers;

public interface IAlphaVantageApiResultMapper
{
    IReadOnlyCollection<SymbolResponse> Map(SymbolApiResult symbolApiResult);
    StockDataResponse Map(StockDataApiResult stockDataApiResult);
}

public class AlphaVantageApiResultMapper : IAlphaVantageApiResultMapper
{
    public IReadOnlyCollection<SymbolResponse> Map(SymbolApiResult symbolApiResult)
        => symbolApiResult.Symbols.Select(item => new SymbolResponse(
            item.Symbol,
            item.Name,
            item.Type,
            item.Region,
            item.MarketOpen,
            item.MarketClose,
            item.Timezone,
            item.Currency,
            Convert.ToDecimal(item.MatchScore, new CultureInfo("en-US"))))
        .ToList();

    public StockDataResponse Map(StockDataApiResult stockDataApiResult)
        => new StockDataResponse(
            new MetaDataResponse(
                stockDataApiResult.MetaDataResponse.Information,
                DateOnly.Parse(stockDataApiResult.MetaDataResponse.LastRefreshed),
                stockDataApiResult.MetaDataResponse.TimeZone),
            Map(stockDataApiResult.StockValues));

    private IEnumerable<StockValueResponse> Map(IReadOnlyDictionary<string, StockValueApiResult> stockValues)
    {
        var result = new List<StockValueResponse>();

        foreach (var kvp in stockValues)
        {
            result.Add(Map(kvp.Key, kvp.Value));
        }

        return result;
    }

    private StockValueResponse Map(string date, StockValueApiResult stockValueApiResult)
        => new StockValueResponse(
            DateOnly.Parse(date),
            Convert.ToDecimal(stockValueApiResult.Open, new CultureInfo("en-US")),
            Convert.ToDecimal(stockValueApiResult.High, new CultureInfo("en-US")),
            Convert.ToDecimal(stockValueApiResult.Low, new CultureInfo("en-US")),
            Convert.ToDecimal(stockValueApiResult.Close, new CultureInfo("en-US")),
            int.Parse(stockValueApiResult.Volume));

}
