using MazeConsumer.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using UnitTests.BaseLibrary;

namespace UnitTests.MazeConsmer.Services;

internal class ScraperServiceBuilder : BuilderBase<ScraperService>
{
    IConfiguration _configuration;

    protected override ScraperService BuildInternal() => 
        new ScraperService(_configuration, Get<IIngestService>(), Get<ILogger<ScraperService>>());

    internal ScraperServiceBuilder WithConfigurationReturning(int numberOfThread)
    {
        var settings = new Dictionary<string, string> {{"NumberOfThread", $"{numberOfThread}"}};

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(settings)
            .Build();
        
        return this;
    }
}
