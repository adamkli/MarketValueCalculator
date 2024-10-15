using MarketValueCalculator.Services.PriceProviders;

namespace MarketValueCalculator.Services;

public interface IPriceProviderFactory
{
    IPriceProvider? Create(string? priceProvider);
    bool IsValid(string? priceProvider);
}
