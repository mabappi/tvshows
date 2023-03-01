using NUnit.Framework;
using RestSharp;

namespace AcceptanceTests.StepDefinitions;

[Binding]
public sealed class TvShowApiStepDefinitions
{
    private const string Result = "Result";
    private readonly ScenarioContext _scenarioContext;

    public TvShowApiStepDefinitions(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [Given(@"TvShow Api is up and running")]
    public void GivenTvShowApiIsUpAndRunning()
    {
        // ToDo: Add check to verify service is running.
    }

    [When(@"API Endpoint is called with page number (.*) and page size (.*)")]
    public void WhenAPIEndpointIsCalledWithPageNumberAndPageSize(int pageNumber, int pageSize)
    {
        var result = GetTvShow(pageNumber, pageSize);
        _scenarioContext[Result] = result;
    }

    [Then(@"Should return empty list")]
    public void ThenShouldReturnEmptyList()
    {
        GetResult().Should().NotBeNull().And.BeEmpty();
    }

    [Then(@"Should return (.*) Tv shows")]
    public void ThenShouldReturnTvShows(int expectedTvShows)
    {
        GetResult().Should().NotBeNull().And.HaveCount(expectedTvShows);
    }

    [Then(@"Cast should be ordered by birthday")]
    public void ThenCastShouldBeOrderedByBirthday()
    {
        var result = GetResult();
        foreach (var item in result)
        {
            item.Cast.Should().BeInAscendingOrder(x => x.Birthday);
        }
    }

    private IEnumerable<TvShow> GetResult() => (IEnumerable<TvShow>)_scenarioContext[Result];

    private IEnumerable<TvShow> GetTvShow(int pageNumber, int pageSize) 
    {
        using var restClient = new RestClient();
        var response = restClient.Execute<IEnumerable<TvShow>>(new RestRequest($"http://localhost:8001/?pageNumber={pageNumber}&pageSize={pageSize}"));
        if (!response.IsSuccessful)
        {
            Assert.Fail(response.ErrorMessage);
        }
        return response.Data;
    }
}