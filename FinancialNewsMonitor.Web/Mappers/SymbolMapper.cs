using FinancialNewsMonitor.DataAccessLayer.Models;
using FinancialNewsMonitor.Responses;

namespace FinancialNewsMonitor.Mappers;

public interface ISymbolMapper
{
    SymbolResponse Map(SymbolModel symbol);
    IEnumerable<SymbolResponse> Map(IEnumerable<SymbolModel> result);
}

public class SymbolMapper : ISymbolMapper
{
    public SymbolResponse Map(SymbolModel symbol)
        => new SymbolResponse(
            symbol.Symbol,
            symbol.Name,
            symbol.Type,
            symbol.Region,
            symbol.MarketOpen,
            symbol.MarketClose,
            symbol.Timezone,
            symbol.Currency,
            symbol.MatchScore);

    public IEnumerable<SymbolResponse> Map(IEnumerable<SymbolModel> result)
        => result.Select(Map);
}
