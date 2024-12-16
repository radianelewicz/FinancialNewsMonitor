using FinancialNewsMonitor.Controllers.Abstracts;
using FinancialNewsMonitor.Cqrs.Queries;
using FinancialNewsMonitor.Mappers;
using FinancialNewsMonitor.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FinancialNewsMonitor.Controllers;

public class ExternalDataController : BaseController<ExternalDataController>
{
    private readonly IAlphaVantageApiResultMapper _alphaVantageApiResultMapper;

    public ExternalDataController(
        IMediator mediator,
        ILogger<ExternalDataController> logger,
        IAlphaVantageApiResultMapper alphaVantageApiResultMapper) : base(mediator, logger)
    {
        _alphaVantageApiResultMapper = alphaVantageApiResultMapper;
    }

    [HttpGet("Symbols/{keyword}")]
    [ProducesResponseType(typeof(IReadOnlyCollection<SymbolResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetSymbolsAsync(
        string keyword,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = _alphaVantageApiResultMapper.Map(
                await _mediator.Send(new GetExternalSymbolSearchQuery(keyword), cancellationToken));

            if (result is null)
            {
                return NotFound();
            }

            if (result.Count == 0)
            {
                return NoContent();
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet("StockData/{symbol}")]
    [ProducesResponseType(typeof(StockDataResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetStockDataAsync(
        string symbol,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = _alphaVantageApiResultMapper.Map(
                await _mediator.Send(new GetExternalStockDataQuery(symbol), cancellationToken));

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
