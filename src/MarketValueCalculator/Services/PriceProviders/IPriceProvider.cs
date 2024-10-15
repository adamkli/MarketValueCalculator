using MarketValueCalculator.Models;
namespace MarketValueCalculator.Services.PriceProviders;
public interface IPriceProvider
{
    string Name { get; }
    Task<IEnumerable<Price>> GetPricesAsync(DateTime startDate, DateTime endDate);
    Task<Price?> GetPriceAsync(string productKey, DateTime date);
}
