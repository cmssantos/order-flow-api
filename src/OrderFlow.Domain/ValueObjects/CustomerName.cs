using OrderFlow.Domain.Exceptions;

namespace OrderFlow.Domain.ValueObjects;

public record CustomerName
{
    public string Value { get; }

    private CustomerName(string value) => Value = value;

    public static CustomerName Create(string value)
    {
        var minLength = FieldLengths.CustomerNameMinLength;
        var maxLength = FieldLengths.CustomerNameMaxLength;

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainValidationException(
                "CustomerName.Empty",
                "The customer name cannot be null or empty."
            );
        }

        if (value.Length < minLength || value.Length > maxLength)
        {
            throw new DomainValidationException(
                "CustomerName.InvalidLength",
                $"The customer name must be between {minLength} and {maxLength} characters long.",
                minLength,
                maxLength
            );
        }

        return new CustomerName(value);
    }
}
