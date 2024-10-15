# Market Value Calculator

This project implements a console application in C# designed to calculate the market value of various financial positions based on current prices from multiple data sources. The application adheres to principles of object-oriented programming, effective use of design patterns, and robust code structuring.

## Overview

The market value is calculated as the product of the price and the amount of the position for a specified date. The user can specify a date range to filter the desired positions and choose the price source provider.

### Example Execution

**Input:**

```bash
~ MarketValueCalculator "source_1" "2020-01-01" "2024-01-01"
```

**Output:**

```plaintext
Product1 on 10.04.2023 Market Value: 1502.50
Product2 on 10.04.2023 Market Value: 1228.75
Product1 on 11.04.2023 Market Value: 2286.00
Product3 on 11.04.2023 Market Value: 4244.80
Product2 on 12.04.2023 Market Value: 1680.00
```

**Arguments**

- The first argument, `"source_1"`, specifies which price provider to use. The application uses an `IPriceProviderFactory` to retrieve the correct provider based on the parameter passed during the application call. 
- The second and third arguments represent the start and end dates for filtering positions. These dates determine the timeframe over which the positions are evaluated. If omitted, the application considers all available data.

## Implementation details
### Input Validation

Parameter validation is handled in the `ConsoleAppArgumentsValidator` class to ensure that input values conform to expected formats and constraints.

### Design Considerations
#### Price Provider Factory

The application features an `IPriceProviderFactory` interface that allows for the implementation of various factories.

To dynamically obtain the appropriate price provider, it employs a `DiscoverablePriceProviderFactory`, which automatically discovers all implementations of the `IPriceProvider` interface within the solution.

The selection of the price provider is based on the Name property defined in the `IPriceProvider` interface. The application currently includes two price provider implementations:

- `Source1PriceProvider`: Utilizes an Entity Framework database context to retrieve prices.
- `Source2PriceProvider`: Generates random price data.

#### Adding New Price Providers

To add new price providers, simply create a new class in the `\Services\PriceProviders` folder that implements the `IPriceProvider` interface. This design allows for easy extension of price source capabilities without modifying existing calculation logic.

### Performance Optimization

To enhance the performance of the `MarketValueCalculatorService`, we employ asynchronous parallel data retrieval from the price provider. This method optimizes the calculation of market values by allowing simultaneous requests for price data, significantly improving the efficiency of the application, especially when dealing with large datasets.

### Testing

This project utilizes **NUnit** and **Moq** frameworks for unit testing. The tests cover:

- **Input Validation**: Verification of the basic functionalities of the `ConsoleAppArgumentsValidator` class to ensure correct validation of input arguments.
- **Application Logic**: Testing of the core logic in the `MarketValueCalculatorService` class to confirm that key operations of the application function as expected.

### Initial Data

#### Sample Data Generation

Sample data for positions and prices is generated in the `\Data\SampleDataGenerator.cs` class. This class contains methods to generate lists of `Position` and `Price` objects.

#### In-Memory Database

The application utilizes an in-memory database configured with `options.UseInMemoryDatabase("MarketValueDatabase")` for data persistence during runtime.


## Conclusion

This implementation demonstrates proficiency in C#, showcasing object-oriented design principles, effective use of patterns, and a well-structured codebase suitable for the task of calculating market values in a financial context. 

The project utilizes the latest solutions available in .NET 8.0 and made certain assumptions beyond the original requirements, which, in a typical scenario, would be considered over-engineering. However, these were intentionally included to highlight the required programming skills.
