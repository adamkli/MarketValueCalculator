namespace MarketValueCalculator.Models;

public class ConsoleAppArguments
{
    public required string[] Args { get; set; }
    public string? PriceProvider { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}