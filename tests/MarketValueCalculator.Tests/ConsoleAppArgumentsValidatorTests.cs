using MarketValueCalculator.Models;
using MarketValueCalculator.Services;
using Moq;
namespace MarketValueCalculator.Tests;

[TestFixture]
public class ConsoleAppArgumentsValidatorTests
{
    private ConsoleAppArgumentsValidator _validator;
    private Mock<IPriceProviderFactory> _mockPriceProviderFactory;
    [SetUp]
    public void Setup()
    {
        _mockPriceProviderFactory = new Mock<IPriceProviderFactory>();
        _validator = new ConsoleAppArgumentsValidator(_mockPriceProviderFactory.Object);
    }

    [TestCase(arg: new[] { "Provider", "2024-10-01", "2024-10-31" })]
    [TestCase(arg: new[] { "Provider" })]
    public void Validate_ValidArguments_ReturnsSuccess(string[] args)
    {
        // Arrange
        _mockPriceProviderFactory.Setup(factory => factory.IsValid(args[0])).Returns(true);

        var options = new ConsoleAppArguments { Args = args };

        // Act
        var result = _validator.Validate(null, options);

        // Assert
        Assert.That(result.Succeeded, Is.True);
        if (args.Length == 3)
        {
            Assert.That(options.StartDate, Is.EqualTo(new DateTime(2024, 10, 1)));
            Assert.That(options.EndDate, Is.EqualTo(new DateTime(2024, 10, 31)));
        }
        else
        {
            Assert.That(options.StartDate, Is.EqualTo(DateTime.MinValue));
            Assert.That(options.EndDate, Is.EqualTo(DateTime.MaxValue));
        }
    }

    [TestCase(arg: new string[] { })]
    [TestCase(arg: new[] { "Provider", "2024-10-01" })]
    [TestCase(arg: new[] { "Provider", "2024-10-01", "2024-10-31", "ExtraArg" })]
    [TestCase(arg: new[] { "Provider", "InvalidDate", "2024-10-31" })]
    [TestCase(arg: new[] { "Provider", "2024-10-01", "InvalidDate" })]
    [TestCase(arg: new[] { "Provider", "2024-10-33", "2024-10-41" })]
    [TestCase(arg: new[] { "Provider", "2024-10-31", "2024-10-01" })]
    public void Validate_InvalidArguments_ReturnsFailure(string[] args)
    {
        // Arrange
        var options = new ConsoleAppArguments { Args = args };

        // Act
        var result = _validator.Validate(null, options);

        // Assert
        Assert.That(result.Succeeded, Is.False);
        Assert.That(result.FailureMessage, Is.Not.Null);
    }

}
