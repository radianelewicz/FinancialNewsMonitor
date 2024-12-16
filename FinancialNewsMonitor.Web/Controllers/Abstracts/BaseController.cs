using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialNewsMonitor.Controllers.Abstracts;

//[Authorize]
[ApiController]
[Route("api/[controller]")]
public abstract class BaseController<TController> : ControllerBase
    where TController : class
{
    protected readonly IMediator _mediator;
    protected readonly ILogger<TController> _logger;

    public BaseController(
        IMediator mediator,
        ILogger<TController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
}
