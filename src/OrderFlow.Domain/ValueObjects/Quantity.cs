using OrderFlow.Domain.Exceptions;

namespace OrderFlow.Domain.ValueObjects;

public record Quantity
{
    public int Value { get; }

    private Quantity(int value) => Value = value;

    public static Quantity Create(int value)
    {
        if (value <= 0)
        {
            throw new DomainValidationException(
                "Quantity.Invalid",
                "Quantity must be greater than zero.",
                value
            );
        }

        return new Quantity(value);
    }
}
