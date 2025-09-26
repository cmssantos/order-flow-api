using OrderFlow.Domain.Exceptions;

namespace OrderFlow.Domain.ValueObjects;

public record StockQuantity
{
    public int Value { get; }

    private StockQuantity(int value) => Value = value;

    public static StockQuantity Create(int value)
    {
        if (value < 0)
        {
            throw new DomainValidationException(
                "Stock.Invalid",
                "Stock cannot be negative.",
                value
            );
        }

        return new StockQuantity(value);
    }

    public StockQuantity Adjust(int adjustment)
    {
        var newValue = Value + adjustment;
        if (newValue < 0)
        {
            throw new DomainValidationException(
                "Stock.Negative",
                "Stock cannot be negative after adjustment.",
                newValue
            );
        }

        return new StockQuantity(newValue);
    }
}
