using MarketValueCalculator.Models;
using MarketValueCalculator.Services;
using MarketValueCalculator.Services.PriceProviders;
using Microsoft.Extensions.Options;
using Moq;

namespace MarketValueCalculator.Tests;

[TestFixture]
public class MarketValueCalculatorServiceTests
{
    private Mock<IPriceProvider> mockPriceProvider;
    private Mock<IPositionRepository> mockPositionRepository;
    private IMarketValueCalculatorService calculatorService;

    [SetUp]
    public void SetUp()
    {
        mockPriceProvider = new Mock<IPriceProvider>();
        mockPositionRepository = new Mock<IPositionRepository>();
        var args = new ConsoleAppArguments
        {
            Args = [],
            PriceProvider = "testprovider",
            StartDate = new DateTime(2023, 04, 10),
            EndDate = new DateTime(2023, 04, 12)
        };
        calculatorService = new MarketValueCalculatorService(mockPriceProvider.Object, mockPositionRepository.Object, Options.Create(args));
    }

    [Test]
    public async Task CalculateMarketValuesAsync_ReturnsEmpty_WhenNoPositions()
    {
        // Arrange
        mockPositionRepository.Setup(pp => pp.GetPositionsAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(new List<Position>()); // No positions returned

        // Act
        var result = await calculatorService.CalculateMarketValuesAsync();

        // Assert
        Assert.That(result, Is.Empty); // Ensure result is empty when no positions exist
    }

    [Test]
    public async Task CalculateMarketValuesAsync_ReturnsMarketValuesWithNull_WhenNoMatchingPrices()
    {
        // Arrange
        var positions = new List<Position>
        {
            new Position { ProductKey = "Product1", Date = new DateTime(2023, 4, 10), Amount = 10 },
            new Position { ProductKey = "Product2", Date = new DateTime(2023, 4, 11), Amount = 5 }
        };

        mockPositionRepository.Setup(pp => pp.GetPositionsAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(positions);

        // Setup for GetPriceAsync to return null since there are no matching prices
        mockPriceProvider.Setup(pp => pp.GetPriceAsync(It.IsAny<string>(), It.IsAny<DateTime>()))
            .ReturnsAsync((string productKey, DateTime date) => null); // Return null for any product key and date


        // Act
        var result = await calculatorService.CalculateMarketValuesAsync();

        // Assert
        Assert.That(result, Is.Not.Empty); // Ensure result is not empty since positions exist
        Assert.That(result.Count(), Is.EqualTo(2)); // We expect two positions returned

        // Check that the result contains the expected values with null
        var expectedResults = new List<MarketValueResult>
        {
            new MarketValueResult { ProductKey = "Product1", Date = new DateTime(2023, 4, 10), Value = null },
            new MarketValueResult { ProductKey = "Product2", Date = new DateTime(2023, 4, 11), Value = null }
        };

        foreach (var expected in expectedResults)
        {
            var actual = result.FirstOrDefault(r => r.ProductKey == expected.ProductKey && r.Date == expected.Date);
            Assert.That(actual, Is.Not.Null); // Ensure we have a corresponding result
            Assert.That(actual.Value, Is.Null); // Check that the market value is null
        }
    }

    [Test]
    public async Task CalculateMarketValuesAsync_ReturnsMarketValues_WhenMatchingPricesExist()
    {
        // Arrange
        var positions = new List<Position>
        {
            new Position { ProductKey = "Product1", Date = new DateTime(2023, 4, 10), Amount = 10 },
            new Position { ProductKey = "Product2", Date = new DateTime(2023, 4, 11), Amount = 5 }
        };

        var prices = new List<Price>
        {
            new Price { ProductKey = "Product1", Date = new DateTime(2023, 4, 10), Value = 100 },
            new Price { ProductKey = "Product2", Date = new DateTime(2023, 4, 11), Value = 200 }
        };

        mockPositionRepository.Setup(pp => pp.GetPositionsAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(positions);

        // Setup for GetPriceAsync to return the correct price based on the product key and date
        mockPriceProvider.Setup(pp => pp.GetPriceAsync("Product1", new DateTime(2023, 4, 10)))
            .ReturnsAsync(prices.First(p => p.ProductKey == "Product1" && p.Date == new DateTime(2023, 4, 10)));

        mockPriceProvider.Setup(pp => pp.GetPriceAsync("Product2", new DateTime(2023, 4, 11)))
            .ReturnsAsync(prices.First(p => p.ProductKey == "Product2" && p.Date == new DateTime(2023, 4, 11)));

        // Act
        var result = await calculatorService.CalculateMarketValuesAsync();

        // Assert
        Assert.That(result, Is.Not.Empty); // Ensure result is not empty
        Assert.That(result.Count(), Is.EqualTo(2)); // We expect two market values returned

        var expectedResults = new List<MarketValueResult>
        {
            new MarketValueResult { ProductKey = "Product1", Date = new DateTime(2023, 4, 10), Value = 1000 }, // 100 * 10
            new MarketValueResult { ProductKey = "Product2", Date = new DateTime(2023, 4, 11), Value = 1000 }  // 200 * 5
        };

        foreach (var expected in expectedResults)
        {
            var actual = result.FirstOrDefault(r => r.ProductKey == expected.ProductKey && r.Date == expected.Date);
            Assert.That(actual, Is.Not.Null); // Ensure we have a corresponding result
            Assert.That(actual.Value, Is.EqualTo(expected.Value)); // Check that the market value matches expected
        }
    }
}
