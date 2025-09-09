using OrderFlow.Domain.Exceptions;

namespace OrderFlow.Domain.ValueObjects;

public record ProductName
{
    public string Value { get; }
    private ProductName(string value) => Value = value;

    public static ProductName Create(string value)
    {
        var minLength = FieldLengths.ProductNameMinLength;
        var maxLength = FieldLengths.ProductNameMaxLength;

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainValidationException(
                "ProductName.Empty",
                "The product name cannot be null or empty."
            );
        }

        if (value.Length < minLength || value.Length > maxLength)
        {
            throw new DomainValidationException(
                "ProductName.InvalidLength",
                $"The product name must be between {minLength} and {maxLength} characters long.",
                minLength,
                maxLength
            );
        }

        return new ProductName(value);
    }
}
