using MarketValueCalculator.Data;
using Microsoft.Extensions.Hosting;
using System.Globalization;
using System.Text;

namespace MarketValueCalculator.Services;

public class ConsoleAppWorker(MarketValueContext context, IMarketValueCalculatorService calculatorService, IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
{
    private readonly CultureInfo cultureInfo = new("pl-PL");
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            context.Database.EnsureCreated();

            var marketValues = await calculatorService.CalculateMarketValuesAsync();

            StringBuilder results = new();
            foreach (var marketValue in marketValues)
            {
                var formattedValue = marketValue.Value.HasValue
                    ? marketValue.Value.Value.ToString("F2", cultureInfo)
                    : "UNKNOWN";
                results.AppendLine($"{marketValue.ProductKey} on {marketValue.Date.ToString("dd.MM.yyyy", cultureInfo)} Market Value: {formattedValue}");
            }
            Console.WriteLine(results.ToString());

        }
        finally
        {
            hostApplicationLifetime.StopApplication();
        }
    }
}