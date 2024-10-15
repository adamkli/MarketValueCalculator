using MarketValueCalculator.Models;
using Microsoft.Extensions.Options;
namespace MarketValueCalculator.Services;
public class ConsoleAppArgumentsValidator(IPriceProviderFactory priceProviderFactory) : IValidateOptions<ConsoleAppArguments>
{
    private const string ExpectedDateFormat = "yyyy-MM-dd";

    public ValidateOptionsResult Validate(string? name, ConsoleAppArguments options)
    {
        var args = options.Args;

        if (!(args.Length == 1 || args.Length == 3))
        {
            return ValidateOptionsResult.Fail("Invalid number of arguments.");
        }

        if (!priceProviderFactory.IsValid(args[0]))
        {
            return ValidateOptionsResult.Fail("Requested Price Provider not found.");
        };

        options.PriceProvider = args[0];

        if (args.Length == 3)
        {

            if (!DateTime.TryParseExact(args[1], ExpectedDateFormat, null, System.Globalization.DateTimeStyles.None, out DateTime startDate) ||
                !DateTime.TryParseExact(args[2], ExpectedDateFormat, null, System.Globalization.DateTimeStyles.None, out DateTime endDate))
            {
                return ValidateOptionsResult.Fail($"Invalid date format provided. Please use the format '{ExpectedDateFormat}'.");
            }

            if (startDate > endDate)
            {
                return ValidateOptionsResult.Fail("The End Date must be after the Start Date. Please check your input.");
            }
            options.StartDate = startDate;
            options.EndDate = endDate;
        }
        else
        {
            options.StartDate = DateTime.MinValue;
            options.EndDate = DateTime.MaxValue;
        }
        return ValidateOptionsResult.Success;
    }
}
