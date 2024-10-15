using MarketValueCalculator.Models;

namespace MarketValueCalculator.Services;

public interface IMarketValueCalculatorService
{
    Task<IEnumerable<MarketValueResult>> CalculateMarketValuesAsync();
}