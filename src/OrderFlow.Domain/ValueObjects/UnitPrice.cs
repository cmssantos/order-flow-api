using OrderFlow.Domain.Exceptions;

namespace OrderFlow.Domain.ValueObjects;

public record UnitPrice
{
    public decimal Value { get; }

    private UnitPrice(decimal value) => Value = value;

    public static UnitPrice Create(decimal value)
    {
        if (value <= 0)
        {
            throw new DomainValidationException(
                "UnitPrice.Invalid",
                "Unit price must be greater than zero.",
                value
            );
        }

        return new UnitPrice(value);
    }
}
