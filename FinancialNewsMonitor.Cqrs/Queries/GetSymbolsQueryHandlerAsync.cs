using FinancialNewsMonitor.DataAccessLayer.Models;
using FinancialNewsMonitor.Services;
using MediatR;

namespace FinancialNewsMonitor.Cqrs.Queries;

public sealed record GetSymbolsQuery() : IRequest<IEnumerable<SymbolModel>>;

public sealed class GetSymbolsQueryHandlerAsync : IRequestHandler<GetSymbolsQuery, IEnumerable<SymbolModel>>
{
    private readonly IFinancialService _financialService;

    public GetSymbolsQueryHandlerAsync(IFinancialService financialService)
    {
        _financialService = financialService;
    }

    public async Task<IEnumerable<SymbolModel>> Handle(GetSymbolsQuery request, CancellationToken cancellationToken)
        => await _financialService.GetSymbolsAsync(cancellationToken);
}
