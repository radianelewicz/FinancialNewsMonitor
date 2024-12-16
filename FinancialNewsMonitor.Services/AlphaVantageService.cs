using FinancialNewsMonitor.Services.Builders;
using FinancialNewsMonitor.Services.Constants;
using FinancialNewsMonitor.Services.ExternalApiModels;
using System.Text.Json;

namespace FinancialNewsMonitor.Services;

public interface IAlphaVantageService
{
    Task<SymbolApiResult> GetSymbolsAsync(string keyword, CancellationToken cancellationToken);
    Task<StockDataApiResult> GetStockDataAsync(string symbol, CancellationToken cancellationToken);
}

//25 requestPerDay
//https://www.alphavantage.co/documentation/
public class AlphaVantageService : IAlphaVantageService
{
    private readonly HttpClient _httpClient;

    public AlphaVantageService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<SymbolApiResult> GetSymbolsAsync(string keyword, CancellationToken cancellationToken)
    {
        var result = await _httpClient.GetStringAsync(
            string.Concat(
                _httpClient.BaseAddress,
                AlphaVantageQueryParams.QUERY_SEGMENT,
                new UriRequestQueryBuilder()
                    .AddQueryParam(AlphaVantageQueryParams.FUNCTION, AlphaVantageQueryParams.FUNCTION_NAME_SYMBOL_SEARCH)
                    .AddQueryParam(AlphaVantageQueryParams.KEYWORDS, keyword)
                    .AddQueryParam(AlphaVantageQueryParams.API_KEY, AlphaVantageQueryParams.API_KEY_VALUE)
                    .Build()),
            cancellationToken);

        if (result == null)
        {
            throw new ArgumentNullException(nameof(result));
        }

        return JsonSerializer.Deserialize<SymbolApiResult>(result)!;
    }

    public async Task<StockDataApiResult> GetStockDataAsync(string symbol, CancellationToken cancellationToken)
    {
        var result = await _httpClient.GetStringAsync(
            string.Concat(
                _httpClient.BaseAddress,
                AlphaVantageQueryParams.QUERY_SEGMENT,
                new UriRequestQueryBuilder()
                    .AddQueryParam(AlphaVantageQueryParams.FUNCTION, AlphaVantageQueryParams.FUNCTION_NAME_TIME_SERIES_DAILY)
                    .AddQueryParam(AlphaVantageQueryParams.SYMBOL, symbol)
                    .AddQueryParam(AlphaVantageQueryParams.API_KEY, AlphaVantageQueryParams.API_KEY_VALUE)
                    .Build()),
            cancellationToken);

        if (result == null)
        {
            throw new ArgumentNullException(nameof(result));
        }

        return JsonSerializer.Deserialize<StockDataApiResult>(result)!;
    }
}
