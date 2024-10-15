using MarketValueCalculator.Models;
using MarketValueCalculator.Services.PriceProviders;
using Microsoft.Extensions.Options;

namespace MarketValueCalculator.Services;

public class MarketValueCalculatorService(IPriceProvider priceProvider, IPositionRepository positionRepository, IOptions<ConsoleAppArguments> options) : IMarketValueCalculatorService
{
    private readonly ConsoleAppArguments args = options.Value;

    public async Task<IEnumerable<MarketValueResult>> CalculateMarketValuesAsync()
    {
        DateTime startDate = args.StartDate;
        DateTime endDate = args.EndDate;

        IEnumerable<Position> positions = await positionRepository.GetPositionsAsync(startDate, endDate);
        var marketValues = new List<MarketValueResult>();

        // Create a list of tasks for getting prices asynchronously
        var tasks = positions
            .Select(async position =>
            {
                var price = await priceProvider.GetPriceAsync(position.ProductKey, position.Date);
                var marketValue = price?.Value * position.Amount;

                return new MarketValueResult
                {
                    ProductKey = position.ProductKey,
                    Date = position.Date,
                    Value = marketValue
                };
            });

        // Execute all tasks concurrently and wait for them to complete
        var results = await Task.WhenAll(tasks);
        marketValues.AddRange(results);

        return marketValues;
    }
}
