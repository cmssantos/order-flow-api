namespace OrderFlow.Domain;

public static class FieldLengths
{
    public const int EmailMinLength = 5;
    public const int EmailMaxLength = 255;

    public const int CustomerNameMinLength = 3;
    public const int CustomerNameMaxLength = 100;

    public const int ProductNameMinLength = 3;
    public const int ProductNameMaxLength = 100;
    public const int ProductSkuMinLength = 3;
    public const int ProductSkuMaxLength = 20;
    public const int ProductDescriptionMinLength = 3;
    public const int ProductDescriptionMaxLength = 200;
    public const int ProductPriceValuePrecision = 18;
    public const int ProductPriceValueScale = 2;
}
