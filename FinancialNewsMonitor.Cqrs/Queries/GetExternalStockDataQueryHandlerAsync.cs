using FinancialNewsMonitor.Services;
using FinancialNewsMonitor.Services.ExternalApiModels;
using MediatR;

namespace FinancialNewsMonitor.Cqrs.Queries;

public sealed record GetExternalStockDataQuery(string Symbol) : IRequest<StockDataApiResult>;

public sealed class GetExternalStockDataQueryHandlerAsync : IRequestHandler<GetExternalStockDataQuery, StockDataApiResult>
{
    private readonly IAlphaVantageService _alphaVantageService;

    public GetExternalStockDataQueryHandlerAsync(IAlphaVantageService alphaVantageService)
    {
        _alphaVantageService = alphaVantageService;
    }

    public async Task<StockDataApiResult> Handle(GetExternalStockDataQuery request, CancellationToken cancellationToken)
        => await _alphaVantageService.GetStockDataAsync(request.Symbol, cancellationToken);
}
