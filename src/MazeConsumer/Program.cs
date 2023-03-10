using Common;
using MazeConsumer.DbContext;
using MazeConsumer.HostedServices;
using MazeConsumer.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console());

builder.Services.AddSingleton<IMazeRestClient, MazeRestClient>();
builder.Services.AddSingleton<IIngestService, IngestService>();
builder.Services.AddSingleton<IScraperService, ScraperService>();
builder.Services.AddSingleton<IScrapperDbContext, ScrapperDbContext>();
builder.Services.AddSingleton<IElasticSearchClient, ElasticSearchClient>();

builder.Services.AddHostedService<ScraperHostedService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();
