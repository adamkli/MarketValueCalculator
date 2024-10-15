using MarketValueCalculator.Services.PriceProviders;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MarketValueCalculator.Services;

public class DiscoverablePriceProviderFactory(IServiceProvider serviceProvider) : IPriceProviderFactory
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    // Caching the provider types
    private List<Type>? _providerTypes;

    private List<Type> GetProviderTypes()
    {
        // Lazily load the provider types
        _providerTypes ??= Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => typeof(IPriceProvider).IsAssignableFrom(t) && !t.IsAbstract)
                .ToList();
        return _providerTypes;
    }

    public IPriceProvider? Create(string? providerName)
    {
        ArgumentNullException.ThrowIfNull(providerName);
        var providerTypes = GetProviderTypes();

        foreach (var type in providerTypes)
        {
            var providerInstance = (IPriceProvider)ActivatorUtilities.CreateInstance(_serviceProvider, type);

            if (providerInstance.Name.Equals(providerName))
            {
                return providerInstance;
            }
        }
        return default;
    }

    public bool IsValid(string? priceProvider)
    {
        return priceProvider != null && Create(priceProvider) != default;
    }
}
