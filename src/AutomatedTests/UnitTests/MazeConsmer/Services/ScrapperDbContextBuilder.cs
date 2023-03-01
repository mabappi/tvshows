using MazeConsumer.DbContext;
using Microsoft.Extensions.Configuration;
using UnitTests.BaseLibrary;

namespace UnitTests.MazeConsmer.Services;

internal class ScrapperDbContextBuilder : BuilderBase<ScrapperDbContext>
{
    private const string DataDir = "data";
    protected override ScrapperDbContext BuildInternal()
        => new ScrapperDbContext(GetConfiguration());
    private IConfiguration GetConfiguration()
    {
        if(!Directory.Exists(DataDir))
            Directory.CreateDirectory(DataDir);
        var settings = new Dictionary<string, string> { { "DataDirectory", DataDir } };

        return new ConfigurationBuilder()
            .AddInMemoryCollection(settings)
            .Build();
    }
}
