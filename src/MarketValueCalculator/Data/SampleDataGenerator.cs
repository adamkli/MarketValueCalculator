using MarketValueCalculator.Models;

namespace MarketValueCalculator.Data;

public static class SampleDataGenerator
{
    public static List<Position> GenerateSamplePositions()
    {
        return new List<Position>
        {
            new Position { Date = new DateTime(2023, 4, 10), ProductKey = "Product1", Amount = 10 },
            new Position { Date = new DateTime(2023, 4, 10), ProductKey = "Product2", Amount = 5 },
            new Position { Date = new DateTime(2023, 4, 11), ProductKey = "Product1", Amount = 15 },
            new Position { Date = new DateTime(2023, 4, 11), ProductKey = "Product3", Amount = 8 },
            new Position { Date = new DateTime(2023, 4, 12), ProductKey = "Product2", Amount = 7 }
        };
    }

    public static List<Price> GenerateSamplePrices()
    {
        return new List<Price>
        {
            new Price { Date = new DateTime(2023, 4, 10), ProductKey = "Product1", Value = 150.25m },
            new Price { Date = new DateTime(2023, 4, 10), ProductKey = "Product2", Value = 245.75m },
            new Price { Date = new DateTime(2023, 4, 11), ProductKey = "Product1", Value = 152.40m },
            new Price { Date = new DateTime(2023, 4, 11), ProductKey = "Product3", Value = 530.60m },
            new Price { Date = new DateTime(2023, 4, 12), ProductKey = "Product2", Value = 240.00m }
        };
    }
}
