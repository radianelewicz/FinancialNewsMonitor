using FinancialNewsMonitor.DataAccessLayer.Models;
using FinancialNewsMonitor.Services;
using MediatR;

namespace FinancialNewsMonitor.Cqrs.Commands;

public sealed record CreateSymbolStockDataCommand(SymbolStockDataModel SymbolStockData) : IRequest<bool>;

public sealed class CreateSymbolStockDataCommandHandlerAsync : IRequestHandler<CreateSymbolStockDataCommand, bool>
{
    private readonly IFinancialService _financialService;

    public CreateSymbolStockDataCommandHandlerAsync(IFinancialService financialService)
    {
        _financialService = financialService;
    }

    public async Task<bool> Handle(CreateSymbolStockDataCommand request, CancellationToken cancellationToken)
        => await _financialService.CreateSymbolStockDataAsync(request.SymbolStockData, cancellationToken);
}
