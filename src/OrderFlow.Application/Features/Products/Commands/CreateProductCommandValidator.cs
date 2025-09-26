using FluentValidation;
using OrderFlow.Domain;

namespace OrderFlow.Application.Features.Products.Commands;

public class CreateProductCommandValidator: AbstractValidator<CreateProductCommand>
{
    private const decimal minPrice = 0.01m;
    private const int minStock = 0;

    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
                .WithMessage("Product name is required.")
            .MinimumLength(FieldLengths.ProductNameMinLength)
                .WithMessage($"Product name must be at least {FieldLengths.ProductNameMinLength} characters long.")
            .MaximumLength(FieldLengths.ProductNameMaxLength)
                .WithMessage($"Product name must not exceed {FieldLengths.ProductNameMaxLength} characters.");

        RuleFor(x => x.Description)
            .NotEmpty()
                .WithMessage("Product description is required.")
            .MinimumLength(FieldLengths.ProductDescriptionMinLength)
                .WithMessage($"Product description must be at least {FieldLengths.ProductDescriptionMinLength} characters long.")
            .MaximumLength(FieldLengths.ProductDescriptionMaxLength)
                .WithMessage($"Product description must not exceed {FieldLengths.ProductDescriptionMaxLength} characters.");

        RuleFor(x => x.Sku)
            .NotEmpty()
                .WithMessage("Product SKU is required.")
            .MinimumLength(FieldLengths.ProductSkuMinLength)
                .WithMessage($"Product SKU must be at least {FieldLengths.ProductSkuMinLength} characters long.")
            .MaximumLength(FieldLengths.ProductSkuMaxLength)
                .WithMessage($"Product SKU must not exceed {FieldLengths.ProductSkuMaxLength} characters.");
        //.Matches(RegexPatterns.ProductSku)
        //.WithMessage("Product SKU may contain only uppercase letters, digits, and hyphens.");

        RuleFor(x => x.Price)
            .GreaterThan(minPrice - 0.01m)
            .WithMessage("Price must be greater than zero.");

        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(minStock)
            .WithMessage("Stock cannot be negative.");
    }
}
