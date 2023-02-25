using IndexingService.Services;
using Microsoft.AspNetCore.Mvc;

namespace IndexingService.Controllers;

[ApiController]
[Route("/")]
public class HomeController : ControllerBase
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public HomeController(IServiceScopeFactory serviceScopeFactory) => _serviceScopeFactory = serviceScopeFactory;

    [HttpGet]
    public IActionResult Index(int pageNumber) 
    {
        _ = Task.Run(async () => 
        { 
            using var scope = _serviceScopeFactory.CreateScope();
            var client = scope.ServiceProvider.GetRequiredService<IElasticSearchClient>();
            await client.Index(pageNumber); 
        });
        return Ok();
    }
}