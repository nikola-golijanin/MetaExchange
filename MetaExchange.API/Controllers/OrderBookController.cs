using System.ComponentModel.DataAnnotations;
using MetaExchange.Shared.Model;
using MetaExchange.Shared.Services;
using Microsoft.AspNetCore.Mvc;

namespace MetaExchange.API.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderBookController : ControllerBase
{
    private readonly IMetaExchangeCalculator _metaExchangeCalculator;
    public OrderBookController(IMetaExchangeCalculator metaExchangeCalculator)

    {
        _metaExchangeCalculator = metaExchangeCalculator;
    }


    [HttpGet("best-execution-plan")]
    public IActionResult GetBestExecutionPlan(
        [FromQuery, Required] OrderType orderType,
        [FromQuery, Range(0.01, double.MaxValue)] double amount)

    {
        var bestExecutionPlan = _metaExchangeCalculator.GetBestExecutionPlanOrderByExchange(orderType, amount);
        return Ok(bestExecutionPlan);
    }
}



