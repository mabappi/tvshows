using Microsoft.AspNetCore.Mvc;
using TvShows.Services;

namespace MazeConsumer.Controllers;

[ApiController]
[Route("/")]
public class HomeController : Controller
{
    private readonly ISearchService _searchService;

    public HomeController(ISearchService searchService) => _searchService = searchService;

    [HttpGet]
    public async Task<IActionResult> Get(int pageNumber, int pageSize) =>
        Json(await _searchService.Search(pageNumber, pageSize));
}