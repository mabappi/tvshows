using MazeConsumer.Services;
using Microsoft.AspNetCore.Mvc;

namespace MazeConsumer.Controllers;

[ApiController]
[Route("/")]
public class HomeController : ControllerBase
{
    private readonly IScraperService _scraperService;

    public HomeController(IScraperService scraperService) => _scraperService = scraperService;

    [HttpGet]
    public IActionResult Status() => Ok(_scraperService.IsRunning);
}