using MarketValueCalculator.Data;
using MarketValueCalculator.Models;
using Microsoft.EntityFrameworkCore;

namespace MarketValueCalculator.Services.PriceProviders;

public class Source1PriceProvider(MarketValueContext context) : IPriceProvider
{
    public string Name => "source_1";

    public async Task<Price?> GetPriceAsync(string productKey, DateTime date)
    {
        return await context.Prices.FindAsync(date, productKey);
    }

    public async Task<IEnumerable<Price>> GetPricesAsync(DateTime startDate, DateTime endDate)
    {
        return await context.Prices
            .AsNoTracking()
            .Where(price => price.Date >= startDate && price.Date <= endDate)
            .ToListAsync();
    }
}
