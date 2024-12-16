using FinancialNewsMonitor.Services;
using FinancialNewsMonitor.Services.ExternalApiModels;
using MediatR;

namespace FinancialNewsMonitor.Cqrs.Queries;

public sealed record GetExternalSymbolSearchQuery(string Keyword) : IRequest<SymbolApiResult>;

public sealed class GetExternalSymbolSearchQueryHandlerAsync : IRequestHandler<GetExternalSymbolSearchQuery, SymbolApiResult>
{
    private readonly IAlphaVantageService _alphaVantageService;

    public GetExternalSymbolSearchQueryHandlerAsync(IAlphaVantageService alphaVantageService)
    {
        _alphaVantageService = alphaVantageService;
    }

    public async Task<SymbolApiResult> Handle(GetExternalSymbolSearchQuery request, CancellationToken cancellationToken)
        => await _alphaVantageService.GetSymbolsAsync(request.Keyword, cancellationToken);
}
