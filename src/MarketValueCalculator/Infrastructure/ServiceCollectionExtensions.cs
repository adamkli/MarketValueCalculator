using MarketValueCalculator.Models;
using MarketValueCalculator.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace MarketValueCalculator.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAndValidateApplicationArguments(this IServiceCollection services, string[] args)
    {
        services.AddSingleton<IValidateOptions<ConsoleAppArguments>, ConsoleAppArgumentsValidator>();
        services.AddOptions<ConsoleAppArguments>()
            .Configure(options => { options.Args = args; })
            .ValidateOnStart();
        return services;
    }
    public static IServiceCollection AddPriceProvider<T>(this IServiceCollection services, string? priceProvider) where T : class, IPriceProviderFactory
    {
        services.AddSingleton<IPriceProviderFactory, T>();
        services.AddSingleton(provider =>
            provider.GetRequiredService<IPriceProviderFactory>().Create(priceProvider)
            ?? throw new ArgumentException("Requested Price Provider not found.")
        );
        return services;
    }
}