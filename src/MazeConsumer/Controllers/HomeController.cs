using MazeConsumer.Models;
using MazeConsumer.Services;
using Microsoft.AspNetCore.Mvc;

namespace MazeConsumer.Controllers;

[ApiController]
[Route("[controller]")]
public class HomeController : ControllerBase
{

    private readonly ILogger<HomeController> _logger;
    private readonly IIngestService _scraperService;

    public HomeController(ILogger<HomeController> logger, IIngestService scraperService)
    {
        _logger = logger;
        _scraperService = scraperService;
    }

    [HttpGet(Name = "Fetch")]
    public async Task<IActionResult> Fetch()
    {
        await _scraperService.Ingest(new ScaperData() { PageNumber= 1 });
        return Ok("Running");
    }
}