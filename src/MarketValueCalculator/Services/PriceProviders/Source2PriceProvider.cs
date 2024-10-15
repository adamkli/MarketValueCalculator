using MarketValueCalculator.Models;
namespace MarketValueCalculator.Services.PriceProviders;

public class Source2PriceProvider : IPriceProvider
{
    public string Name => "source_2";

    private static readonly string[] ProductKeys = { "Product1", "Product2", "Product3" };
    private readonly Random _random;

    public Source2PriceProvider() => _random = new Random();

    public async Task<IEnumerable<Price>> GetPricesAsync(DateTime startDate, DateTime endDate)
    {
        // simulate delay while reading from "source_2"
        await Task.Delay(1000);

        var prices = new List<Price>();
        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {
            foreach (var product in ProductKeys)
            {
                prices.Add(GenerateRandomPrice(date, product));
            }
        }
        return prices;
    }

    public Task<Price?> GetPriceAsync(string productKey, DateTime date)
    {
        var price = GenerateRandomPrice(date, productKey);
        return Task.FromResult<Price?>(price);
    }

    private Price GenerateRandomPrice(DateTime date, string productKey)
    {
        decimal randomValue = Math.Round((decimal)(_random.NextDouble() * (300 - 100) + 100), 2);
        return new Price
        {
            Date = date,
            ProductKey = productKey,
            Value = randomValue
        };
    }
}
