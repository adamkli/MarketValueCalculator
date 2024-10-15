namespace MarketValueCalculator.Models;
public class MarketValueResult
{
    public required string ProductKey { get; set; }
    public DateTime Date { get; set; }
    public decimal? Value { get; set; }
}
