using MarketValueCalculator.Data;
using MarketValueCalculator.Models;
using Microsoft.EntityFrameworkCore;

namespace MarketValueCalculator.Services;

public class PositionRepository(MarketValueContext context) : IPositionRepository
{
    public async Task<IEnumerable<Position>> GetPositionsAsync(DateTime startDate, DateTime endDate)
    {
        return await context.Positions
            .AsNoTracking()
            .Where(p => p.Date >= startDate && p.Date <= endDate)
            .ToListAsync();
    }
}