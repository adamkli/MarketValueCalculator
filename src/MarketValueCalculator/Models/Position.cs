namespace MarketValueCalculator.Models;
/// <summary>
/// Represents amount of certain financial product that was present in client's portfolio at certain date
/// </summary>
public class Position
{
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public required string ProductKey { get; set; }
}
