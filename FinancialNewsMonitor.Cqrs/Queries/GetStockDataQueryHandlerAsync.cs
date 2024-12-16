using FinancialNewsMonitor.DataAccessLayer.Models;
using FinancialNewsMonitor.Services;
using MediatR;

namespace FinancialNewsMonitor.Cqrs.Queries;

public sealed record GetStockDataQuery(string Symbol) : IRequest<StockDataModel>;

public sealed class GetStockDataQueryHandlerAsync : IRequestHandler<GetStockDataQuery, StockDataModel>
{
    private readonly IFinancialService _financialService;

    public GetStockDataQueryHandlerAsync(IFinancialService financialService)
    {
        _financialService = financialService;
    }

    public async Task<StockDataModel> Handle(GetStockDataQuery request, CancellationToken cancellationToken)
        => await _financialService.GeStockDataAsync(request.Symbol, cancellationToken);
}
