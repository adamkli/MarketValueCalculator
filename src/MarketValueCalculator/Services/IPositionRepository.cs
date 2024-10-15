using MarketValueCalculator.Models;

namespace MarketValueCalculator.Services;

public interface IPositionRepository
{
    Task<IEnumerable<Position>> GetPositionsAsync(DateTime startDate, DateTime endDate);
}
