using FinancialNewsMonitor.Services;
using MediatR;

namespace FinancialNewsMonitor.Cqrs.Commands;

public sealed record DeleteSymbolStockDataCommand(string Symbol) : IRequest<bool>;

public sealed class DeleteSymbolStockDataCommandHandlerAsync : IRequestHandler<DeleteSymbolStockDataCommand, bool>
{
    private readonly IFinancialService _financialService;

    public DeleteSymbolStockDataCommandHandlerAsync(
        IFinancialService financialService)
    {
        _financialService = financialService;
    }

    public async Task<bool> Handle(DeleteSymbolStockDataCommand request, CancellationToken cancellationToken)
        => await _financialService.DeleteAsync(request.Symbol, cancellationToken);
}
