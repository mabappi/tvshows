using Microsoft.AspNetCore.Mvc;

namespace MazeConsumer.Controllers;

[ApiController]
[Route("[controller]")]
public class HomeController : ControllerBase
{

    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "Fetch")]
    public async Task<IActionResult> Fetch()
    {
        return Ok("Running");
    }
}