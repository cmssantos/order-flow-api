using OrderFlow.Domain.Exceptions;

namespace OrderFlow.Domain.ValueObjects;

public record Sku
{
    public string Value { get; }

    private Sku(string value) => Value = value;

    public static Sku Create(string value)
    {
        var minLength = FieldLengths.ProductSkuMinLength;
        var maxLength = FieldLengths.ProductSkuMaxLength;

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainValidationException(
                "ProductSKU.Empty",
                "The SKU cannot be null or empty."
            );
        }

        if (value.Length < minLength || value.Length > maxLength)
        {
            throw new DomainValidationException(
                "ProductSKU.InvalidLength",
                $"The product sku must be between {minLength} and {maxLength} characters long.",
                minLength,
                maxLength
            );
        }

        return new Sku(value);
    }
}
