using MarketValueCalculator.Data;
using MarketValueCalculator.Infrastructure;
using MarketValueCalculator.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder();

builder.Logging.ClearProviders();
//builder.Logging.AddFilter(level => level >= LogLevel.Warning);

builder.Services.AddDbContext<MarketValueContext>(options =>
    options.UseInMemoryDatabase("MarketValueDatabase"));
builder.Services.AddTransient<IPositionRepository, PositionRepository>();

builder.Services.AddAndValidateApplicationArguments(args);
builder.Services.AddPriceProvider<DiscoverablePriceProviderFactory>(args.FirstOrDefault());

builder.Services.AddTransient<IMarketValueCalculatorService, MarketValueCalculatorService>();
builder.Services.AddHostedService<ConsoleAppWorker>();

var host = builder.Build();

try
{
    await host.RunAsync();
}
catch (Exception e)
{
    Console.WriteLine($"Error: {e.Message}");
    DisplayUsage();
}

static void DisplayUsage()
{
    Console.WriteLine("Usage: ~ MarketValueCalculator \"source_1\" \"2020-01-01\" \"2024-01-01\"");
    Console.WriteLine("- \"source_1\": Specify the price provider (e.g., \"source_1\", \"source_2\").");
    Console.WriteLine("- \"2020-01-01\" and \"2024-01-01\": Start and end dates for filtering positions.");
}