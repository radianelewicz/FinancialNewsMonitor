using FinancialNewsMonitor.Controllers.Abstracts;
using FinancialNewsMonitor.Cqrs.Commands;
using FinancialNewsMonitor.Cqrs.Queries;
using FinancialNewsMonitor.Mappers;
using FinancialNewsMonitor.Requests;
using FinancialNewsMonitor.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FinancialNewsMonitor.Controllers;

public class FinancialController : BaseController<FinancialController>
{
    private readonly IStockDataModelMapper _stockDataModelMapper;
    private readonly ISymbolStockDataMapper _symbolStockDataMapper;
    private readonly ISymbolMapper _symbolMapper;

    public FinancialController(
        IMediator mediator,
        ILogger<FinancialController> logger,
        IStockDataModelMapper stockDataModelMapper,
        ISymbolStockDataMapper symbolStockDataMapper,
        ISymbolMapper symbolMapper) : base(mediator, logger)
    {
        _stockDataModelMapper = stockDataModelMapper;
        _symbolStockDataMapper = symbolStockDataMapper;
        _symbolMapper = symbolMapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SymbolResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new GetSymbolsQuery(), cancellationToken);

            if (result is null
                || result.Count() == 0)
            {
                return NoContent();
            }

            return Ok(_symbolMapper.Map(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet("{symbol}")]
    [ProducesResponseType(typeof(StockDataResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetAsync(string symbol, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new GetStockDataQuery(symbol), cancellationToken);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(_stockDataModelMapper.Map(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.Found)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> PostAsync([FromBody] CreateSymbolStockDataRequest createSymbolStockDataRequest, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(
                new CreateSymbolStockDataCommand(
                    _symbolStockDataMapper.Map(createSymbolStockDataRequest)),
                cancellationToken);

            if (!result)
            {
                //legitnie redirect na GETa albo RedirectToAction -> wymaga dodanie endpointa z pobraniem SymbolStockDataResponse
                return StatusCode((int)HttpStatusCode.Found);
            }

            return Created();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPatch]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> PatchAsync([FromBody] UpdateSymbolStockDataRequest updateSymbolStockDataRequest, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(
                new UpdateSymbolStockDataCommand(
                    _symbolStockDataMapper.Map(updateSymbolStockDataRequest)),
                cancellationToken);

            if (!result)
            {
                return NotFound(result);
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpDelete("{symbol}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> DeleteAsync(string symbol, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(new DeleteSymbolStockDataCommand(symbol), cancellationToken);

            if (!result)
            {
                return NotFound(result);
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
