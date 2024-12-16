using FinancialNewsMonitor.DataAccessLayer.Models;
using FinancialNewsMonitor.Services;
using MediatR;

namespace FinancialNewsMonitor.Cqrs.Commands;

public sealed record UpdateSymbolStockDataCommand(SymbolStockDataModel SymbolStockData) : IRequest<bool>;

public sealed class UpdateSymbolStockDataCommandHandlerAsync : IRequestHandler<UpdateSymbolStockDataCommand, bool>
{
    private readonly IFinancialService _financialService;

    public UpdateSymbolStockDataCommandHandlerAsync(IFinancialService financialService)
    {
        _financialService = financialService;
    }

    public async Task<bool> Handle(UpdateSymbolStockDataCommand request, CancellationToken cancellationToken)
        => await _financialService.UpdateSymbolStockDataAsync(request.SymbolStockData, cancellationToken);
}
