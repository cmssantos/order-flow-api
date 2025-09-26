using OrderFlow.Domain.Exceptions;

namespace OrderFlow.Domain.ValueObjects;

public record ProductDescription
{
    public string Value { get; }

    private ProductDescription(string value) => Value = value;

    public static ProductDescription Create(string value)
    {
        var minLength = FieldLengths.ProductDescriptionMinLength;
        var maxLength = FieldLengths.ProductDescriptionMaxLength;

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainValidationException(
                "ProductDescription.Empty",
                "The product description cannot be null or empty."
            );
        }

        if (value.Length < minLength || value.Length > maxLength)
        {
            throw new DomainValidationException(
                "ProductDescription.InvalidLength",
                $"The product description must be between {minLength} and {maxLength} characters long.",
                minLength,
                maxLength
            );
        }

        return new ProductDescription(value);
    }
}
